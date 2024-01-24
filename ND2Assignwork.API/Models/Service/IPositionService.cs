using ND2Assignwork.API.Models.Domain;
using ND2Assignwork.API.Models.DTO;
using ND2Assignwork.API.Models.DTO.DTO_Identity;

namespace ND2Assignwork.API.Models.Service
{
    public interface IPositionService
    {
        IEnumerable<PositionDTO> GetAllPositions();
        PositionDTO GetPositionById(int id);
        int CreatePosition(PositionDTO_Identity positionDTO);
        bool UpdatePosition(PositionDTO positionDTO);
        bool DeletePosition(int id);





        Task<Position> GetPositionByIdAsnyc(int id);
    }
}
