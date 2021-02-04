using IMDbApi.Context;
using IMDbApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMDb.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly dbContext _context;

        public MoviesController(dbContext context)
        {
            _context = context;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        [Route("Cadastro")]
        [Authorize(Roles = "isAdmin")]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _context.movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        [Route("Voto")]
        public async Task<ActionResult<Movie>> RateMovie(Rating rate)
        {
            _context.ratings.Add(rate);
            await _context.SaveChangesAsync();

            return Ok("Votado");
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("ObterFilmesPorDiretor")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetmoviesByDirector(string diretor)
        {
            return await _context.movies.Where(f => f.Director == diretor).ToListAsync();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("ObterFilmesPorNome")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetmoviesByName(string nomeFilme)
        {
            return await _context.movies.Where(f => f.Name.Contains(nomeFilme)).ToListAsync();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("ObterFilmesPorGenero")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetmoviesByGenre(string genero)
        {
            return await _context.movies.Where(g => g.Genres.Any(m => m.Name.Contains(genero))).ToListAsync();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("ObterFilmesPorAtores")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetmoviesByActors(string actor)
        {
            return await _context.movies.Where(g => g.Actors.Any(m => m.Name.Contains(actor))).ToListAsync();
        }
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }


    }
}
