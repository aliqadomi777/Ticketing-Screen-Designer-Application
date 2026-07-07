using System.Collections.Generic;
using Ticketing_Screen_Designer.DTO.ButtonTypes;
using Ticketing_Screen_Designer.Interfaces.Repositories;
using Ticketing_Screen_Designer.Interfaces.Services;
using Ticketing_Screen_Designer.Models;
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
            var buttonType = _fetchRepository.GetById(typeId);
            return new ButtonTypeResponseDto
            {
                TypeId = buttonType.TypeId,
                TypeName = buttonType.TypeName
            };
        }

        public List<ButtonTypeResponseDto> GetAllButtonTypes()
        {
            List<ButtonTypeResponseDto> buttonTypesList = new List<ButtonTypeResponseDto>();
            var buttonTypes = _fetchAllRepository.GetAll();
            foreach (var buttonType in buttonTypes)
            {
                buttonTypesList.Add(new ButtonTypeResponseDto
                {
                    TypeId = buttonType.TypeId,
                    TypeName = buttonType.TypeName
                });
            }
            return buttonTypesList;
        }
    }
}
