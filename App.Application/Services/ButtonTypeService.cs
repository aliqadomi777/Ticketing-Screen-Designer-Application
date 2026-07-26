using App.Application.DTO.ButtonTypes;
using App.Application.Interfaces;
using App.Domain.Interfaces;
using App.Domain.Models;
using App.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
namespace App.Application.Services
{
    public class ButtonTypeService : IButtonTypeService
    {
        private readonly IFetchableRepository<ButtonTypes> _fetchRepository;
        private readonly IGetAllRepository<ButtonTypes> _fetchAllRepository;
        private readonly ILogger<ButtonTypeService> _logger;

        public ButtonTypeService(IFetchableRepository<ButtonTypes> fetchRepository,
            IGetAllRepository<ButtonTypes> fetchAllRepository,
            ILogger<ButtonTypeService> logger)
        {
            _fetchRepository = fetchRepository;
            _fetchAllRepository = fetchAllRepository;
            _logger = logger;
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
                _logger.LogError(ex, "Failed executing query for button type with ID: {typeId}", typeId);
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
                _logger.LogError(ex, "Failed executing query retrieving all button types: {bankId}");
                throw new DataAccessException("Could not retrieve All button types", ex);
            }


        }
    }
}
