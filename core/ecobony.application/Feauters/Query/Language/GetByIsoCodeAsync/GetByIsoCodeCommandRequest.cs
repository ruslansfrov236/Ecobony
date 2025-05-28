namespace ecobony.application.Feauters.Query.Language;

public class GetByIsoCodeCommandRequest:IRequest<GetByIsoCodeCommandResponse>
{
    public string isoCode { get; set; }
}