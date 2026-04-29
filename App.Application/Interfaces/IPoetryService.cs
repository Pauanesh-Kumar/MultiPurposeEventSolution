using App.Application.DTOs.Request;
using App.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Interfaces
{
    public interface IPoetryService
    {
        Task<List<PoemDto>?> GetPoemDetailAsync(int autherId);
        Task<bool> SaveAsync(CreatePoemDto createPoemDto);
        Task<bool> UpdateAsync(ModifyPoemDto modifyPoemDto);
        Task<bool> DeleteAsync(int id);
    }
}
