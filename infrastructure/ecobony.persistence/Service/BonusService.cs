

using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Http;

namespace ecobony.persistence.Service; 
public class BonusService(
    IBonusComunityReadRepository _bonusComunityRead,
    IBonusComunityWriteRepository _bonusComunityWrite,
    IBonusReadRepository _bonusRead,
    IBonusWriteRepository _bonusWrite,
    ICategoryReadRepository _categoryRead,
    ILanguageJsonService languageJsonService,
    IWasteReadRepository _wasteRead,
    UserManager<AppUser> _userManager,
    IHttpContextAccessor _contextAccessor,
    ILanguageReadRepository _languageRead,
    IMemoryCache _memoryCache
        ):IBonusService
{
    public async Task<List<BonusComunity>> GetAdminAll()
    {
      
        var latestUpdateAt = await _bonusComunityRead.GetFilter
                (x => !x.isDeleted)
            .MaxAsync(x =>
                (DateTime?)(
                    (x.UpdateAt > x.CreateAt ? x.UpdateAt : x.CreateAt))
            );

        string cacheKey = $"bonus{latestUpdateAt}";
        if (_memoryCache.TryGetValue(cacheKey, out (DateTime lastUpdate, List<BonusComunity> data) cachedData))
        {
            if (cachedData.lastUpdate>=latestUpdateAt)
            {
                return cachedData.data; 
            }
        }

        var bonus = await _bonusComunityRead.GetAll().Include(a => a.Bonus).ToListAsync();
        
        _memoryCache.Set(cacheKey, (latestUpdateAt , bonus), new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });
        return bonus;
    }

    public async Task<List<BonusComunity>> GetClientAll()
    {
        var languageCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

        var language = await _languageRead.GetSingleAsync(a => a.IsoCode == languageCode && a.isDeleted==false);

        var latestUpdateAt = await _bonusComunityRead.GetFilter
                (x => !x.isDeleted)
            .MaxAsync(x =>
                (DateTime?)(
                    (x.UpdateAt > x.CreateAt ? x.UpdateAt : x.CreateAt))
            );

        string cacheKey = $"bonus{language.Id}";
        if (_memoryCache.TryGetValue(cacheKey, out (DateTime lastUpdate, List<BonusComunity> data) cachedData))
        {
            if (cachedData.lastUpdate>=latestUpdateAt)
            {
                return cachedData.data; 
            }
        }

        var username =  _contextAccessor?.HttpContext?.User?.Identity?.Name;
        if (string.IsNullOrEmpty(username))
            throw new UnauthorizedAccessException(languageJsonService.LanguageStrongJson("Unauthorized"));

        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new NotFoundException();

        var bs = await _bonusRead.GetSingleAsync(a => a.UserId == user.Id && a.isDeleted==false) ?? throw new NotFoundException(languageJsonService.LanguageStrongJson("NotFound"));
        var bonus = await _bonusComunityRead.GetAll().Include(a => a.Bonus).Where(a=>a.BonusId==bs.Id && a.isDeleted==false).ToListAsync();
        
        _memoryCache.Set(cacheKey, (latestUpdateAt , bonus), new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });

        return bonus;
    }

    public async Task<FileStreamResult> GetClientExcelView(DateTime? StartDate , DateTime? EndDate)
{
    
  

    var username = _contextAccessor?.HttpContext?.User?.Identity?.Name;
    if (string.IsNullOrEmpty(username))
        throw new UnauthorizedAccessException(languageJsonService.LanguageStrongJson("Unauthorized"));

    var user = await _userManager.FindByNameAsync(username)
               ?? throw new NotFoundException(languageJsonService.LanguageStrongJson("UserNotFound"));

    var bonus = await _bonusRead.GetSingleAsync(a => a.isDeleted == false
                                                     && a.UserId == user.Id
                                                     && a.CreateAt >= StartDate && a.CreateAt < EndDate)
                 ?? throw new NotFoundException();

    var bonusComunity = await _bonusComunityRead
        .GetSingleAsync(a => a.isDeleted == false
                             && a.BonusId == bonus.Id)
        ?? throw new NotFoundException();

    var wasteList = await _wasteRead
        .GetFilter(w => w.isDeleted == false
                        && w.UserId == user.Id
                        && bonusComunity.WasteId == w.Id)
        .ToListAsync();

    if (wasteList == null || !wasteList.Any())
        throw new NotFoundException(languageJsonService.LanguageStrongJson("NoWasteFound"));

    using var workbook = new XLWorkbook();
    var worksheet = workbook.Worksheets.Add(languageJsonService.LanguageStrongJson("WasteReport"));

    // Header
    worksheet.Cell(1, 1).Value = "ID";
    worksheet.Cell(1, 2).Value = "Title";
    worksheet.Cell(1, 3).Value = "Station";
    worksheet.Cell(1, 4).Value = "Score";
    worksheet.Cell(1, 5).Value = "Created Date";

    int row = 2;
    foreach (var w in wasteList)
    {
        worksheet.Cell(row, 1).Value = w.Id.ToString();
        worksheet.Cell(row, 2).Value = w.Title;
        worksheet.Cell(row, 3).Value = w.Station;
        worksheet.Cell(row, 4).Value = bonusComunity.Score;
        worksheet.Cell(row, 5).Value = w.CreateAt.ToString("yyyy-MM-dd");
        row++;
    }

    worksheet.Columns().AdjustToContents();

    var stream = new MemoryStream();
    workbook.SaveAs(stream);
    stream.Position = 0;

    return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
    {
        FileDownloadName = "waste-report.xlsx"
    };
}

    public async Task<FileStreamResult> GetClinetPdfView(DateTime? StartDate , DateTime? EndDate)
    {
     
        
        var document = new Document();
    var username = _contextAccessor?.HttpContext?.User?.Identity?.Name;
    if (string.IsNullOrEmpty(username))
        throw new UnauthorizedAccessException("User not authenticated");

    var user = await _userManager.FindByNameAsync(username)
               ?? throw new NotFoundException("User not found");

    var bonus = await _bonusRead.GetSingleAsync(a => a.isDeleted == false
                                                     && a.UserId == user.Id &&
                                a.CreateAt >= StartDate && a.CreateAt < EndDate)
                 ?? throw new NotFoundException();

    var bonusComunity = await _bonusComunityRead
        .GetSingleAsync(a => a.isDeleted == false
                             && a.BonusId == bonus.Id)
        ?? throw new NotFoundException();

    var wasteList = await _wasteRead
        .GetFilter(w => w.isDeleted == false
                        && w.UserId == user.Id
                        && bonusComunity.WasteId == w.Id)
        .ToListAsync();

    if (wasteList == null || !wasteList.Any())
        throw new NotFoundException("No waste found");

    Section section = document.AddSection();

    var paragraph = section.AddParagraph();
    paragraph.AddFormattedText("Waste Details", TextFormat.Bold);
    paragraph.AddLineBreak();

    foreach (var w in wasteList)
    {
        paragraph.AddText($"Title: {w.Title}");
        paragraph.AddLineBreak();
        paragraph.AddText($"Station: {w.Station}");
        paragraph.AddLineBreak();
        paragraph.AddText($"Score: {bonusComunity.Score}");
        paragraph.AddLineBreak();
        paragraph.AddText($"User: {user.UserName}");
        paragraph.AddLineBreak();
        paragraph.AddText($"Report Period: {StartDate:yyyy-MM-dd} - {EndDate:yyyy-MM-dd}");
        paragraph.AddLineBreak();

        paragraph.AddLineBreak();
    }

    var table = section.AddTable();
    table.Borders.Width = 0.75;

    table.AddColumn("1cm");
    table.AddColumn("5cm");
    table.AddColumn("5cm");
    table.AddColumn("5cm");

    var headerRow = table.AddRow();
    headerRow.Cells[0].AddParagraph("ID");
    headerRow.Cells[1].AddParagraph("Name");
    headerRow.Cells[2].AddParagraph("Station");
    headerRow.Cells[3].AddParagraph("Created Date");

    foreach (var w in wasteList)
    {
        var row = table.AddRow();
        row.Cells[0].AddParagraph(w.Id.ToString());
        row.Cells[1].AddParagraph(w.Title);
        row.Cells[2].AddParagraph(w.Station);
        row.Cells[3].AddParagraph(w.CreateAt.ToString("yyyy-MM-dd"));
    }

    var footer = section.Footers.Primary.AddParagraph();
    footer.AddText($"Generated by: {user.UserName} - {DateTime.Now:yyyy-MM-dd}");
    footer.Format.Alignment = ParagraphAlignment.Center;

    // Export PDF
    PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();
    pdfRenderer.Document = document;
    pdfRenderer.RenderDocument();

    using var stream = new MemoryStream();
    pdfRenderer.PdfDocument.Save(stream, false);
    stream.Position = 0;

    return new FileStreamResult(stream, "application/pdf")
    {
        FileDownloadName = "waste-report.pdf"
    };
}

    public async Task<bool> Create(string wasteId)
    {
        var httpContext = _contextAccessor.HttpContext;
        if (!Guid.TryParse(wasteId, out _))
            throw new BadRequestException(languageJsonService.LanguageStrongJson("InvalidGuid"));

        var username = _contextAccessor?.HttpContext?.User?.Identity?.Name;
        if (string.IsNullOrEmpty(username))
            throw new CustomUnauthorizedException(languageJsonService.LanguageStrongJson("Unauthorized"));

        var user = await _userManager.FindByNameAsync(username)
                   ?? throw new NotFoundException(languageJsonService.LanguageStrongJson("UserNotFound"));

        var waste = await _wasteRead.GetByIdAsync(wasteId)
                    ?? throw new NotFoundException(languageJsonService.LanguageStrongJson("WasteNotFound"));

        var bonus = await _bonusRead.GetSingleAsync(a => a.UserId == user.Id && a.isDeleted==false);
        var idempotencyKey = httpContext.Request.Headers["Idempotency-Key"].FirstOrDefault();

        if (string.IsNullOrEmpty(idempotencyKey))
        {
            throw new BadRequestException(languageJsonService.LanguageStrongJson("IdempotencyKeyRequired"));
         
        }
        if (bonus is null)
        {
            bonus = new Bonus
            {
                UserId = user.Id
            };
            await _bonusWrite.AddAsync(bonus);
            await _bonusWrite.SaveChangegesAsync();
        }

      
        
        var bonusCommunity = await _bonusComunityRead.GetSingleAsync(bc => bc.BonusId == bonus.Id && bc.isDeleted==false );

        if (bonusCommunity is null)
        {
            bonusCommunity = new BonusComunity
            {

                BonusId = bonus.Id,
                Score = waste.Score,
                PricePoint= waste.PricePoint,
                IdempotencyKey= idempotencyKey,
            };
            await _bonusComunityWrite.AddAsync(bonusCommunity);
        }
        else
        {
            bonusCommunity.Score += waste.Score;
            bonusCommunity.PricePoint += waste.PricePoint;
            bonusCommunity.IdempotencyKey = idempotencyKey;
            _bonusComunityWrite.Update(bonusCommunity); 
        }

        await _bonusComunityWrite.SaveChangegesAsync();
        return true;
    }

     
    

    public async Task<bool> SoftDelete(string id)
    {
        if (!Guid.TryParse(id, out _))
            throw new BadRequestException(languageJsonService.LanguageStrongJson("InvalidGuid"));

        var bonus = await _bonusRead.GetSingleAsync(a => a.Id == Guid.Parse(id) && a.isDeleted==false) ?? throw new NotFoundException();

        bonus.isDeleted = true;

        _bonusWrite.Update(bonus);
       await _bonusWrite.SaveChangegesAsync();
        var bonusComunity = await _bonusComunityRead.GetSingleAsync(a => a.BonusId == bonus.Id && !a.isDeleted==false) ?? throw new NotFoundException();
        bonusComunity.isDeleted = true;
        _bonusComunityWrite.Update(bonusComunity);
      await  _bonusComunityWrite.SaveChangegesAsync();

        return true;
    }

    public async Task<bool> RestoreDelete(string id)
    {
        if (!Guid.TryParse(id, out _))
            throw new BadRequestException(languageJsonService.LanguageStrongJson("InvalidGuid"));

        var bonus = await _bonusRead.GetSingleAsync(a => a.Id == Guid.Parse(id) && a.isDeleted==true) ?? throw new NotFoundException();

        bonus.isDeleted=false;

        _bonusWrite.Update(bonus);
      await  _bonusWrite.SaveChangegesAsync();
        var bonusComunity = await _bonusComunityRead.GetSingleAsync(a => a.BonusId == bonus.Id && a.isDeleted==true)
            ?? throw new NotFoundException(languageJsonService.LanguageStrongJson("NotFound"));
        bonusComunity.isDeleted=false;
        _bonusComunityWrite.Update(bonusComunity);
     await   _bonusComunityWrite.SaveChangegesAsync();

        return true;
    }

    public async Task<bool> Delete(string id)
    {
        if (!Guid.TryParse(id, out _))
            throw new BadRequestException(languageJsonService.LanguageStrongJson("InvalidGuid"));

        var bonus = await _bonusRead.GetSingleAsync(a => a.Id == Guid.Parse(id) && a.isDeleted==true) 
            ?? throw new NotFoundException(languageJsonService.LanguageStrongJson("NotFound"));

    
        _bonusWrite.Delete(bonus);
     await   _bonusWrite.SaveChangegesAsync();

        return true;
    }
}