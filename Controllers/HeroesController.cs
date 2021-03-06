using HeroesApi.Data;
using HeroesApi.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HeroesApi
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HeroesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ApiConfiguration configuration;

        public HeroesController(ApplicationDbContext dbContext, IOptions<ApiConfiguration> configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration.Value;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<Hero>>> Get()
        {
            var heroes = await dbContext.Heroes.ToListAsync();
            foreach (var hero in heroes)
            {
                SetHeroTrainingToday(hero);
            }

            return heroes;
        }

        [HttpGet("{heroId}")]
        public async Task<ActionResult<Hero>> GetHero(Guid heroId)
        {
            var hero = await dbContext.Heroes.FindAsync(heroId);
            if (hero == null)
            {
                return NotFound();
            }
            SetHeroTrainingToday(hero);
            return hero;
        }

        [HttpPost]
        public async Task<ActionResult<Hero>> CreateHero([FromBody] Hero hero)
        {
            dbContext.Heroes.Add(hero);
            hero.TrainerName = this.HttpContext.User.Identity.Name;
            await dbContext.SaveChangesAsync();

            var dbHero = await GetHero(hero.Id);
            return dbHero;
        }

        [HttpPut("{heroId}")]
        public async Task<ActionResult<Hero>> UpdateHero(Guid heroId, [FromBody] Hero hero)
        {
            var dbHero = await dbContext.Heroes.FindAsync(heroId);
            if (dbHero == null)
            {
                return NotFound();
            }
            dbContext.Entry(dbHero).CurrentValues.SetValues(hero);
            await dbContext.SaveChangesAsync();

            var newDbHero = await GetHero(heroId);
            return newDbHero;
        }

        [HttpPut("{heroId}/training")]
        public async Task<ActionResult<Hero>> TrainHero(Guid heroId)
        {
            var hero = await dbContext.Heroes.FindAsync(heroId);
            if (hero == null)
            {
                return NotFound();
            }
            SetHeroTrainingToday(hero);
            if (hero.NumTrainingToday >= configuration.MaxTrainingPerDay)
            {
                return Conflict(new ApiError
                {
                    Code = ApiErrors.HERO_ALREADY_TRAINED,
                    Message = "The hero has been trained enough today"
                });
            }
            if (hero.TrainerName != HttpContext.User.Identity.Name)
            {
                return Conflict(new ApiError
                {
                    Code = ApiErrors.HERO_BELONGS_TO_OTHER_TRAINER,
                    Message = "You can't train other trainer's hero"
                });
            }

            hero.CurrentPower *= 1 + (new Random().NextDouble() / 10);
            hero.NumTrainingAtLastDate = hero.NumTrainingToday + 1;
            hero.LastTrainingDate = DateTime.Now;
            hero.FirstTrainingDate = hero.FirstTrainingDate ?? hero.LastTrainingDate;
            await dbContext.SaveChangesAsync();

            var updatedHero = await GetHero(heroId);
            return updatedHero;
        }

        private void SetHeroTrainingToday(Hero hero)
        {
            hero.NumTrainingToday = (hero.LastTrainingDate?.Date ?? DateTime.MinValue) < DateTime.Now.Date ?
                0 :
                hero.NumTrainingAtLastDate;
        }
    }
}
