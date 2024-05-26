using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Results;
using System.Threading.Tasks;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IGameService
    {
        Task<ServiceResult<GameResult>> PerformAction(string playerId);
        Task<ServiceResult<BuyResult>> Buy(string playerId, int itemId);
    }
}
