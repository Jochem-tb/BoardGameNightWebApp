﻿using BGN.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Services.Abstractions
{
    public interface IUserService
    {
        Task<PersonDto?> GetLoggedInUserAsync();
    }
}
