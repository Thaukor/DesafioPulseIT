using DesafioPulseIT.Models;
using Microsoft.AspNetCore.Mvc;

namespace DesafioPulseIT.Controllers
{
    public class DataObject
    {
        public int id { get; set; }
        public IFormCollection collection { get; set; }
    }

    public class InvitadoController : Controller
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private const string eventoEndPoint = "api/evento";
        private const string invitadoEndPoint = "api/invitado";
        private const string baseUrl = "https://localhost:7179";

        // GET: InvitadoController
        public ActionResult Index ()
        {
            return View();
        }

        public async Task<ActionResult> Edit ( int id )
        {
            // Conseguir la invitación con el id
            string invitadoUrl = $"{baseUrl}/{invitadoEndPoint}/details/{id}";
            string eventoUrl = $"{baseUrl}/{eventoEndPoint}/details/{id}";
            var response = await httpClient.GetAsync(invitadoUrl);

            if ( response.IsSuccessStatusCode )
            {
                // Conseguir la invitación
                Invitado? invitado = await response.Content.ReadFromJsonAsync<Invitado>();
                // Conseguir el evento
                response = await httpClient.GetAsync( eventoUrl );

                if ( response.IsSuccessStatusCode )
                {
                    // Guardar el evento
                    Evento? evento = await response.Content.ReadFromJsonAsync<Evento>();
                    ViewBag.datosEvento = evento;
                }

                return View( "VerInvitacion", invitado );
            }

            return BadRequest( $"Error al encontrar la invitación de id {id}" );
        }

        // POST: InvitadoController/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit ( int id, IFormCollection collection )
        {
            string invitadoUrl = $"{baseUrl}/{invitadoEndPoint}/edit";

            Invitado invitado = new Invitado()
            {
                Id = id,
                Asiste = collection["asistencia"] == "1",
                Email = "",
                EventoId = -1
            };

            var response = await httpClient.PostAsJsonAsync( invitadoUrl, invitado );
            if (response.IsSuccessStatusCode )
            {
                await Console.Out.WriteLineAsync("Respondido exitosamente");
            }
            return RedirectToAction( nameof( Index ) );
        }
    }
}
