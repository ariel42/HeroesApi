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
        private readonly AppConfiguration configuration;

        public HeroesController(ApplicationDbContext dbContext, IOptions<AppConfiguration> configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration.Value;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<Hero>>> Get()
        {
            try
            {
                var heroes = await dbContext.Heroes.ToListAsync();
                foreach (var hero in heroes)
                {
                    SetHeroTrainingToday(hero);
                }
                
                return heroes;
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("{heroId}")]
        public async Task<ActionResult<Hero>> GetHero(Guid heroId)
        {
            try
            {
                var hero = await dbContext.Heroes.FindAsync(heroId);
                if (hero == null)
                {
                    return NotFound();
                }
                SetHeroTrainingToday(hero);
                return hero;
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Hero>> CreateHero(Hero hero)
        {
            try
            {
                dbContext.Heroes.Add(hero);
                await dbContext.SaveChangesAsync();
                var dbHero = await dbContext.Heroes.FindAsync(hero.Id);
                return dbHero;
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("{heroId}")]
        public async Task<ActionResult<Hero>> UpdateHero(Guid heroId, [FromBody] Hero hero)
        {
            try
            {
                var dbHero = await dbContext.Heroes.FindAsync(heroId);
                if (dbHero == null)
                {
                    return NotFound();
                }
                dbContext.Heroes.Update(hero);
                await dbContext.SaveChangesAsync();
                var newDbHero = await dbContext.Heroes.FindAsync(hero.Id);
                SetHeroTrainingToday(newDbHero);
                return newDbHero;
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("{heroId}/training")]
        public async Task<ActionResult<Hero>> TrainHero(Guid heroId)
        {
            try
            {
                var hero = await dbContext.Heroes.FindAsync(heroId);
                if (hero == null)
                {
                    return NotFound();
                }
                SetHeroTrainingToday(hero);
                if (hero.NumTrainingToday >= configuration.MaxTrainingPerDay)
                {
                    return Conflict("The hero has been trained enough today");
                }

                hero.CurrentPower *= 1 + (new Random().NextDouble() / 10);
                hero.NumTrainingAtLastDate = hero.NumTrainingToday + 1;
                hero.LastTrainingDate = DateTime.Now;
                hero.FirstTrainingDate = hero.FirstTrainingDate ?? hero.LastTrainingDate;
                var updatedHero = await UpdateHero(heroId, hero);
                return updatedHero;
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        private void SetHeroTrainingToday(Hero hero)
        {
            hero.NumTrainingToday = (hero.LastTrainingDate?.Date ?? DateTime.MinValue) < DateTime.Now.Date ?
                0 :
                hero.NumTrainingAtLastDate;
        }
    }
}
