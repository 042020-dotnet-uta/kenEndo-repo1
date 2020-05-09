using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcGames.Data;
using MvcGames.Models;


namespace MvcGames.Data
{
    public class Queries
    {
        private readonly MvcGameContext _context;

        public Queries(MvcGameContext context) {
            _context = context;
        }
        public IQueryable<string> getCity() {
            IQueryable<string> cityQuery = from c in _context.Location select c.City;
            return cityQuery;
        }

        public IQueryable<string> getGenre() {
            IQueryable<string> genreQuery = from g in _context.Game select g.Genre;
            return genreQuery;
        }

        public IQueryable<Game> getGameName(string searchString, string genre, string city) {
            //var gameQuery = from g in _context.Game join l in _context.Location on g.locationID equals l.locationID select g;
            var gameQuery = from g in _context.Game select g;

            if (!string.IsNullOrEmpty(searchString)) {
                gameQuery = gameQuery.Where(g => g.Title.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(city))
            {
                var cityId = _context.Location.First(x => x.City == city).locationID;
                gameQuery = gameQuery.Where(g => g.locationID == cityId);
            }
            if (!string.IsNullOrEmpty(genre)) {
                gameQuery = gameQuery.Where(g => g.Genre.Contains(genre));
            }
           
            return gameQuery;
        }
    }
}
