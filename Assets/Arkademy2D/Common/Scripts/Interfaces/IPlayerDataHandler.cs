using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Arkademy2D.Common.Interfaces
{
    public interface IPlayerDataHandler
    {
        Task<IList<PlayerData>> GetAllPlayerDataAsync(CancellationToken cancellationToken);
        Task<PlayerData> LoadPlayerDataAsync(Guid playerGuid, CancellationToken cancellationToken);
        Task SavePlayerDataAsync(PlayerData playerData, CancellationToken cancellationToken);
    }
}