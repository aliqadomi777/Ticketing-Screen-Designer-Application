using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using Ticketing_Screen_Designer.DTO.ButtonTypes;
using Ticketing_Screen_Designer.Interfaces.Repositories;
using Ticketing_Screen_Designer.Interfaces.Services;
using Ticketing_Screen_Designer.Models;
using Ticketing_Screen_Designer.Utils;
namespace Ticketing_Screen_Designer.Services
{
    public class ButtonTypeService : IButtonTypeService
    {
        private readonly IFetchableRepository<ButtonTypes> _fetchRepository;
        private readonly IGetAllRepository<ButtonTypes> _fetchAllRepository;
        public ButtonTypeService(IFetchableRepository<ButtonTypes> fetchRepository, IGetAllRepository<ButtonTypes> fetchAllRepository)
        {
            _fetchRepository = fetchRepository;
            _fetchAllRepository = fetchAllRepository;
        }

        public ButtonTypeResponseDto GetButtonType(int typeId)
        {
            if (typeId <= 0)
            {
                throw new ArgumentException("type ID must be a positive non-zero integer.", nameof(typeId));
            }

            try
            {
                var buttonType = _fetchRepository.GetById(typeId);
                return new ButtonTypeResponseDto
                {
                    TypeId = buttonType.TypeId,
                    TypeName = buttonType.TypeName
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed executing query for button type with ID: {typeId}", typeId);
                throw new DataAccessException($"Could not retrieve profile records for Type ID {typeId}.", ex);
            }
        }

        public List<ButtonTypeResponseDto> GetAllButtonTypes()
        {
            try
            {
                var buttonTypes = _fetchAllRepository.GetAll();
                return buttonTypes.Select(buttonType => new ButtonTypeResponseDto
                {
                    TypeId = buttonType.TypeId,
                    TypeName = buttonType.TypeName
                }).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed executing query retrieving all button types: {bankId}");
                throw new DataAccessException("Could not retrieve All button types", ex);
            }


        }
    }
}
