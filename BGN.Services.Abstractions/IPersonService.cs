﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Services.Abstractions
{
    public interface IPersonService
    {
        Task<IEnumerable<PersonDto>> GetAllAsync();
        Task<PersonDto> GetByIdAsync(int id);
        Task<PersonDto> CreateAsync(PersonDto person);
        Task<PersonDto> UpdateAsync(int id, PersonDto person);
        Task DeleteAsync(int id);
    }
}
