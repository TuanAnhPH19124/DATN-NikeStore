using Domain.Entities;
using EntitiesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Abstractions
{
    public interface IAppUserService
    {
        Task<AppUser> CreateAsync(AppUserForCreateDto appUserForCreationDto, CancellationToken cancellationToken = default);
    }
}
