using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeroesApi
{
    public enum ApiErrors
    {
        LOGIN_ERROR = 1,
        SIGNUP_ERROR,
        HERO_ALREADY_TRAINED,
        HERO_BELONGS_TO_OTHER_TRAINER
    }

    public class ApiError
    {
        public ApiErrors Code { get; set; }
        public string Message { get; set; }
    }
}
