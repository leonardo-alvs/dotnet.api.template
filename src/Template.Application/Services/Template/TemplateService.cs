using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Template.Infra.Data.SqlServer;
using Template.Application.Dto.Template;
using Template.Application.Services.Shared;
using Template.Application.Shared.Notifications;
using Template.Domain.Entities.Template;
using Template.Infra;

using TemplateTestsDomain = Template.Domain.Entities.Template.Template;

namespace Template.Application.Services.Template;

public class TemplateService : BaseService, ITemplateService
{
    private readonly IMapper _mapper;
    private readonly ISqlServerUnitOfWork _uow;

    public TemplateService(NotificationContext notificationContext, IMapper mapper, ISqlServerUnitOfWork uow) : base(notificationContext)
    {
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<TemplateDto> GetTemplateById(int id)
        => _mapper.Map<TemplateDto>(
            await _uow.TemplateRepository.GetById(id));

    public async Task<TemplateDto?> CreateTemplate(TemplateDto dto)
    {
        var template = _mapper.Map<TemplateTestsDomain>(dto);

        ValidateDomainEntity(template, new TemplateValidator());

        if (NotificationContext.HasNotifications)
            return null;

        await _uow.TemplateRepository.Create(template);
        await _uow.CommitContextAsync();

        return _mapper.Map<TemplateDto>(template);
    }

    public async Task DeleteTemplate(int id)
    {
        if (!await _uow.TemplateRepository.Delete(id))
        {
            NotificationContext.AddNotification(nameof(ApplicationValidationMessages.EntityNotFound),
                string.Format(ApplicationValidationMessages.EntityNotFound, id));
            return;
        }

        await _uow.CommitContextAsync();
    }

    public async Task<TemplateDto?> UpdateTemplate(int id, TemplateDto dto)
    {
        var template = await _uow.TemplateRepository.GetById(id);

        if (template == null)
        {
            NotificationContext.AddNotification(nameof(ApplicationValidationMessages.EntityNotFound),
                string.Format(ApplicationValidationMessages.EntityNotFound, id));
            return null;
        }

        template.Update(_mapper.Map<TemplateTestsDomain>(dto));
        _uow.TemplateRepository.Update(template);

        await _uow.CommitContextAsync();

        return _mapper.Map<TemplateDto>(template);
    }

    public async Task<ICollection<TemplateDto>> GetAllTemplates()
        => _mapper.Map<ICollection<TemplateDto>>(
            await _uow.TemplateRepository.GetAll().ToListAsync());

    public async Task<ICollection<TemplateDto>> GetActiveTemplates()
        => _mapper.Map<ICollection<TemplateDto>>(
            await _uow.TemplateRepository.GetActiveTemplates());
}