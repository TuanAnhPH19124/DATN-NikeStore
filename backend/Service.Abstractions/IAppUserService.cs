using Domain.Entities;
using DTOConvert;
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
        Task<AppUser> CreateAsync(AppUserForCreationDTO userForCreationDto, CancellationToken cancellationToken = default);
    }
}
