using ecobony.application.Repository;
using ecobony.domain.Dto_s;
using ecobony.domain.Entities;
using ecobony.signair.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.signair.Services
{
    public class TrashCanSignairService
    {
        readonly private ITrashCanWriteRepository _trashCanWriteRepository;
        readonly private IHubContext<TrashCanHub> _hubContext;
        public TrashCanSignairService(ITrashCanWriteRepository? trashCanWriteRepository, IHubContext<TrashCanHub>? hubContext)
        {
            _trashCanWriteRepository = trashCanWriteRepository;
            _hubContext = hubContext;
        }


    
        public async Task SendTaskCreate(CreateTrashCanDto_s model )
        {

            var trashCan = new TrashCan()
            {
                Id= Guid.NewGuid(),
                EntityId = model.EntityId,
                EntityName = model.EntityName,
                OperationAt = model.OperationAt,
                OperationType = model.OperationType,
                UserId = model.UserId,
                UserName = model.UserName,
               
                
                
            };
            await _trashCanWriteRepository.AddAsync(trashCan);
            await _trashCanWriteRepository.SaveChangegesAsync();
            await _hubContext.Clients.All.SendAsync(ReceiveHubName.ReceiveTrashCan, trashCan);
        }
        public async Task SendTaskDeleteCreate(CreateTrashCanDto_s model)
        {

            var trashCan = new TrashCan()
            {
                EntityId = model.EntityId,
                EntityName = model.EntityName,
                OperationAt = model.OperationAt,
                OperationType = model.OperationType,
                UserId = model.UserId,
                UserName = model.UserName,
                DeletedAt = model.DeletedAt



            };
            await _trashCanWriteRepository.AddAsync(trashCan);
            await _trashCanWriteRepository.SaveChangegesAsync();
            await _hubContext.Clients.All.SendAsync(ReceiveHubName.ReceiveTrashCan, trashCan);
        }
    }
}
