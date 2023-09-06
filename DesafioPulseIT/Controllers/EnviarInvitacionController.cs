using DesafioPulseIT.Models;
using DesafioPulseITAPI.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace DesafioPulseIT.Controllers
{
	public class EnviarInvitacionController : Controller
	{
		private static readonly HttpClient httpClient = new HttpClient();
		private const string eventoEndPoint = "api/evento";
		private const string invitadoEndPoint = "api/invitado";
		private const string correosEndPoint = "api/correo";
		private const string baseUrl = "https://localhost:7179";

		[HttpGet]
		public async Task<ActionResult> Enviar ( int id )
		{
			string eventoUrl = $"{baseUrl}/{eventoEndPoint}/details/{id}";
			string invitadoUrl = $"{baseUrl}/{invitadoEndPoint}/create";

			// Conseguir datos del evento
			HttpResponseMessage response = await httpClient.GetAsync(eventoUrl);

			if ( response.IsSuccessStatusCode )
			{
				Evento evento = await response.Content.ReadFromJsonAsync<Evento>();

				return View( evento );
			}

			return BadRequest( "Error" );
		}

		[HttpPost]
		public async Task<ActionResult> EnviarCorreo ( int id, IFormCollection collection )
		{
			string invitadoUrl = $"{baseUrl}/{invitadoEndPoint}/create";
			string correosUrl = $"{baseUrl}/{correosEndPoint}/enviar";

			// Conseguir datos del evento
			if ( int.TryParse( collection[ "id" ], out int eventoID ) )
			{
				// Crear invitaciones
				string? correos = collection["emails"];

				foreach ( string correo in correos.Split( ";" ) )
				{
					// Crear invitación al invitado específico
					Invitado invitado = new Invitado()
					{
						Email = correo,
						Asiste = false,
						Respondio = false,
						EventoId = eventoID
					};

					// Enviar request a la API
					HttpResponseMessage responseInv = await httpClient.PostAsJsonAsync( invitadoUrl, invitado );

					// Leer respuestas
					if ( responseInv.IsSuccessStatusCode )
					{
						var idInvitacion = await responseInv.Content.ReadAsStringAsync();
						await Console.Out.WriteLineAsync( $"Invitación con ID {idInvitacion} creada con éxito" );

						// Crear correo
						Correo correoObj = new Correo()
						{
							De = "claudiotestpulse@outlook.com",
							Para = correo,
							Msg = $"Has sido invitado a un evento. Puedes responder aquí: https://localhost:7225/invitado/edit/{idInvitacion}",
							Asunto = "Invitación",
							Email = "claudiotestpulse@outlook.com",
							Contrasena = "Claudio123456"
						};
						// Enviar correos
						responseInv = await httpClient.PostAsJsonAsync( correosUrl, correoObj );

						if ( responseInv.IsSuccessStatusCode )
						{
							await Console.Out.WriteLineAsync( "Correo enviado exitosamente" );
						}
					}
					else
					{
						await Console.Out.WriteLineAsync( $"Error al crear invitado con correo {invitado.Email} en el evento de ID: {eventoID}" );
					}
				}

				return RedirectToAction( nameof( Index ) );
			}
			else
			{
				return BadRequest( "Error" );
			}

		}
	}
}
