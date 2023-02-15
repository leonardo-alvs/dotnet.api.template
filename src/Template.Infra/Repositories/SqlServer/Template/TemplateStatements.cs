namespace Template.Infra.Repositories.SqlServer.Template;
public static class TemplateStatements
{
    public const string GetActives = @"SELECT 
                                        CD_TEMPLATE_TESTS Id, 
                                        DS_DESCRIPTION Description, 
                                        FL_ACTIVE Active, 
                                        DT_INSERTION InsertionDate 
                                    FROM 
                                        TB_TEMPLATE_TESTS 
                                    WHERE 
                                        FL_ACTIVE = 1";
}
