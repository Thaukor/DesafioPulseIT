using DesafioPulseIT.Models;
using DesafioPulseITAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DesafioPulseIT.Controllers
{
	public class EventoController : Controller
	{
		private static readonly HttpClient httpClient = new HttpClient();
		private const string eventoEndPoint = "api/evento";
		private const string invitadoEndPoint = "api/invitado";
		private const string correosEndPoint = "api/correo";
		private const string baseUrl = "https://localhost:7179";

		// GET: EventoController
		[HttpGet]
		public async Task<ActionResult> Index ()
		{
			List <Evento> eventos;
			string eventoUrl = $"{baseUrl}/{eventoEndPoint}/index";

			HttpResponseMessage response = await httpClient.GetAsync(eventoUrl);
			if ( response.IsSuccessStatusCode )
			{
				eventos = await response.Content.ReadFromJsonAsync<List<Evento>>();

				return View( eventos );
			}
			return BadRequest( "Ocurrió un error al contactar la API" );
		}

		// GET: EventoController/Details/5
		public ActionResult Details ( int id )
		{
			return View();
		}

		// GET: EventoController/Create
		public ActionResult Create ()
		{
			return View( "GenerarEvento" );
		}

		// POST: EventoController/Create
		[HttpPost]
		public async Task<ActionResult> Create ( IFormCollection collection )
		{
			string eventoUrl = baseUrl + "/" + eventoEndPoint + "/create";
			string invitadoUrl = baseUrl + "/" + invitadoEndPoint + "/create";

			if ( DateTime.TryParse( collection[ "fecha" ], out var fecha ) )
			{
				Evento evento = new Evento()
				{
					Titulo = collection[ "titulo" ],
					Descripcion = collection["descripcion"],
					Fecha = fecha
				};
				HttpResponseMessage response = await httpClient.PostAsJsonAsync(eventoUrl, evento);

				if ( response.IsSuccessStatusCode )
				{
					return RedirectToAction( nameof( Index ) );
				}
				else
				{
					// No pudo crear el evento
					return BadRequest( "Error al crear evento" );
				}

			}


			return View( "GenerarEvento" );
		}

		// GET: EventoController/Edit/5
		public ActionResult Edit ( int id )
		{
			return View();
		}

		// POST: EventoController/Edit/5
		[HttpPost]
		public ActionResult Edit ( int id, IFormCollection collection )
		{
			try
			{
				return RedirectToAction( nameof( Index ) );
			}
			catch
			{
				return View();
			}
		}

		// GET: EventoController/Delete/5
		public ActionResult Delete ( int id )
		{
			return View();
		}

		// POST: EventoController/Delete/5
		[HttpPost]
		public ActionResult Delete ( int id, IFormCollection collection )
		{
			try
			{
				return RedirectToAction( nameof( Index ) );
			}
			catch
			{
				return View();
			}
		}
	}
}
