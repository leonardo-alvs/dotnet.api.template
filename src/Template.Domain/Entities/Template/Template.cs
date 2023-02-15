using Template.Domain.Entities.Shared;
using Template.Domain.Entities.Template;

namespace Template.Domain.Entities.Template;

public class Template : Entity
{
    public string Description { get; set; }
    public bool Active { get; set; }
    public DateTime InsertionDate { get; set; }

    public Template()
    {
    }

    public void Update(Template templateUpdate)
    {
        Description = templateUpdate.Description;
        Active = templateUpdate.Active;

        Validate(this, new TemplateValidator());
    }
}