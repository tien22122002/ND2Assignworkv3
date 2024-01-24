using Microsoft.EntityFrameworkCore;
using ND2Assignwork.API.Data;
using ND2Assignwork.API.Models.Domain;
using ND2Assignwork.API.Models.DTO;
using ND2Assignwork.API.Models.DTO.DTO_Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ND2Assignwork.API.Models.Service.Imp
{
    public class PositionService : IPositionService
    {
        private readonly DataContext _context;

        public PositionService(DataContext context)
        {
            this._context = context;
        }

        public IEnumerable<PositionDTO> GetAllPositions()
        {
            return _context.Position.Select(p => new PositionDTO
            {
                Position_Id = p.Position_Id,
                Position_Name = p.Position_Name,
            }).ToList();
        }

        public PositionDTO GetPositionById(int id)
        {
            var positionEntity = _context.Position.Find(id);
            if (positionEntity == null)
            {
                return null;
            }

            return new PositionDTO
            {
                Position_Id = positionEntity.Position_Id,
                Position_Name = positionEntity.Position_Name,
            };
        }
        public async Task<Position> GetPositionByIdAsnyc(int id)
        {
            var position = await _context.Position.FindAsync(id);
            if (position == null)
            {
                return null;
            }
            return position;
        }

        public int CreatePosition(PositionDTO_Identity positionDTO)
        {
            var positionEntity = new Position
            {
                Position_Name = positionDTO.Position_Name,
            };

            _context.Position.Add(positionEntity);
            try
            {
                _context.SaveChanges();
                return positionEntity.Position_Id;
            }
            catch (Exception ex)
            {
                return 0;
            }
            
        }

        public bool UpdatePosition(PositionDTO positionDTO)
        {
            var positionEntity = _context.Position.Find(positionDTO.Position_Id);
            if (positionEntity == null)
            {
                return false;
            }

            positionEntity.Position_Name = positionDTO.Position_Name;
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeletePosition(int id)
        {
            var positionEntity = _context.Position.Find(id);
            if (positionEntity == null)
            {
                return false;
            }

            _context.Position.Remove(positionEntity);
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }



}
