namespace ecobony.application.Feauters.Command;

public class DeleteBonusCommandHandler(IBonusService _bonusService):IRequestHandler<DeleteBonusCommandRequest, DeleteBonusCommandResponse>
{
    public async Task<DeleteBonusCommandResponse> Handle(DeleteBonusCommandRequest request, CancellationToken cancellationToken)
    {
      await  _bonusService.Delete(request.Id);
      return new DeleteBonusCommandResponse();
    }
}