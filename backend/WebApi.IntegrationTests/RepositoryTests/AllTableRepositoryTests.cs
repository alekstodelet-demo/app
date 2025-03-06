using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Application.Repositories;
using Dapper;
using Domain.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Hosting;
using Moq;
using SQLitePCL;

public class AllTableRepositoryTests
{
    private List<string> ignoreInsertionList = new()
    {
        "SubscribtionContactType",
        "IntervalForSchedule"
    };
    
    private List<string> ignoreUpdateList = new()
    {
        "SubscribtionContactType",
        "IntervalForSchedule",
        "File",
    };
    
    List<(string TableName, Type EntityType)> tableDefinitions = new()
    {
        ("CustomSubscribtion", typeof(CustomSubscribtion)),
        ("custom_svg_icon", typeof(CustomSvgIcon)),
        ("IntervalForSchedule", typeof(IntervalForSchedule)),
        ("Language", typeof(Language)),
        ("RepeatType", typeof(RepeatType)),
        ("Role", typeof(Role)),
        ("S_DocumentTemplate", typeof(S_DocumentTemplate)),
        ("S_DocumentTemplateTranslation", typeof(S_DocumentTemplateTranslation)),
        ("S_DocumentTemplateType", typeof(S_DocumentTemplateType)),
        ("S_PlaceHolderTemplate", typeof(S_PlaceHolderTemplate)),
        ("S_PlaceHolderType", typeof(S_PlaceHolderType)),
        ("S_QueriesDocumentTemplate", typeof(S_QueriesDocumentTemplate)),
        ("S_Query", typeof(S_Query)),
        ("S_TemplateDocumentPlaceholder", typeof(S_TemplateDocumentPlaceholder)),
        ("ScheduleType", typeof(ScheduleType)),
        ("SubscriberType", typeof(SubscriberType)),
        ("SubscribtionContactType", typeof(SubscribtionContactType)),
        ("User", typeof(User)),
        ("application", typeof(Domain.Entities.Application)),
        ("application_comment", typeof(application_comment)),
        ("application_document", typeof(ApplicationDocument)),
        ("application_document_type", typeof(ApplicationDocumentType)),
        ("application_object", typeof(application_object)),
        ("application_paid_invoice", typeof(ApplicationPaidInvoice)),
        ("application_payment", typeof(application_payment)),
        ("application_road", typeof(ApplicationRoad)),
        ("application_status", typeof(ApplicationStatus)),
        ("application_status_history", typeof(ApplicationStatusHistory)),
        ("application_subtask", typeof(application_subtask)),
        ("application_subtask_assignee", typeof(application_subtask_assignee)),
        ("application_task", typeof(application_task)),
        ("application_task_assignee", typeof(application_task_assignee)),
        ("application_work_document", typeof(ApplicationWorkDocument)),
        ("arch_object", typeof(ArchObject)),
        ("arch_object_tag", typeof(arch_object_tag)),
        ("archive_log", typeof(ArchiveLog)),
        ("archive_log_status", typeof(ArchiveLogStatus)),
        ("archive_object", typeof(ArchiveObject)),
        ("archive_object_file", typeof(ArchiveObjectFile)),
        // ("archive_object_tag", typeof(archive_object_tag)),
        ("contact_type", typeof(contact_type)),
        ("contragent", typeof(contragent)),
        ("contragent_interaction", typeof(contragent_interaction)),
        ("contragent_interaction_doc", typeof(contragent_interaction_doc)),
        ("customer", typeof(Customer)),
        ("customer_contact", typeof(customer_contact)),
        ("customer_representative", typeof(CustomerRepresentative)),
        ("district", typeof(District)),
        // ("document_metadata", typeof(DocumentMetadata)),
        ("employee", typeof(Employee)),
        ("employee_contact", typeof(employee_contact)),
        ("employee_event", typeof(EmployeeEvent)),
        ("employee_in_structure", typeof(EmployeeInStructure)),
        ("faq_question", typeof(faq_question)),
        ("file", typeof(Domain.Entities.File)),
        ("file_for_application_document", typeof(FileForApplicationDocument)),
        ("file_type_for_application_document", typeof(FileTypeForApplicationDocument)),
        ("hrms_event_type", typeof(HrmsEventType)),
        ("identity_document_type", typeof(identity_document_type)),
        ("notification", typeof(notification)),
        ("notification_log", typeof(notification_log)),
        ("notification_template", typeof(notification_template)),
        ("org_structure", typeof(OrgStructure)),
        ("org_structure_templates", typeof(org_structure_templates)),
        ("organization_type", typeof(organization_type)),
        ("saved_application_document", typeof(saved_application_document)),
        // ("saved_document_metadata", typeof(saved_document_metadata)),
        ("service", typeof(Service)),
        ("service_document", typeof(ServiceDocument)),
        // ("service_source", typeof(service_source)),
        ("structure_application_log", typeof(structure_application_log)),
        ("structure_post", typeof(StructurePost)),
        ("tag", typeof(Tag)),
        ("task_status", typeof(task_status)),
        ("task_type", typeof(task_type)),
        ("uploaded_application_document", typeof(uploaded_application_document)),
        ("user_role", typeof(UserRole)),
        ("work_day", typeof(WorkDay)),
        ("work_schedule", typeof(WorkSchedule)),
        ("work_schedule_exception", typeof(WorkScheduleException)),
        ("workflow", typeof(Workflow)),
        ("workflow_subtask_template", typeof(WorkflowSubtaskTemplate)),
        ("workflow_task_dependency", typeof(WorkflowTaskDependency)),
        ("workflow_task_template", typeof(WorkflowTaskTemplate)),
    };

    [Fact]
    public async Task Verify_BulkTableInsertion_ShouldPassForAllTables()
    {
        var connection = CreateInMemoryDatabase(tableDefinitions);

        foreach (var (tableName, entityType) in tableDefinitions)
        {
            if (ignoreInsertionList.Any(ignore => entityType.Name.Contains(ignore)))
            {
                return;
            }
            var userRepositoryMock = new Mock<IUserRepository>();
            var userID = 7;
            userRepositoryMock.Setup(repo => repo.GetUserID()).ReturnsAsync(userID);
            var testEntity = CreateTestEntity(entityType);

            var repository = CreateRepositoryForEntity(entityType, connection, userRepositoryMock.Object);

            var addMethod = repository.GetType().GetMethod("Add");
            Assert.True(addMethod != null, $"{entityType.Name} is missing an add method.");

            int resultId = 0;
            if (addMethod.GetParameters().Length == 1)
            {
                resultId = await (Task<int>)addMethod.Invoke(repository, new[] { testEntity });
            }else if (addMethod.GetParameters().Length == 2 && addMethod.GetParameters()[1].ParameterType == typeof(SubscribtionContactType))
            {
                SubscribtionContactType? testContactType = new SubscribtionContactType
                { 
                    idTypeContact = Array.Empty<int>()
                };
                resultId = await (Task<int>)addMethod.Invoke(repository, new object[] { testEntity, testContactType });
            }

            Assert.True(resultId > 0);

            var insertedRecord = await connection.QueryFirstOrDefaultAsync(
                $"SELECT * FROM {tableName} WHERE id = @id",
                new { id = resultId });

            Assert.NotNull(insertedRecord);
            var recordDictionary = (IDictionary<string, object>)insertedRecord;

            try
            {
                Assert.True(recordDictionary.ContainsKey("created_at") && recordDictionary["created_at"] != null,
                    $"{entityType.Name} created_at is null");
                Assert.True(recordDictionary.ContainsKey("updated_at") && recordDictionary["updated_at"] != null,
                    $"{entityType.Name} updated_at is null");
                Assert.True(
                    recordDictionary.ContainsKey("created_by") &&
                    int.Parse(recordDictionary["created_by"].ToString()) == userID,
                    $"{entityType.Name} created_by is null");
                Assert.True(
                    recordDictionary.ContainsKey("updated_by") &&
                    int.Parse(recordDictionary["updated_by"].ToString()) == userID,
                    $"{entityType.Name} updated_by is null");
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to insert {tableName} records: {e.Message}");
            }
        }
    }

    [Fact]
    public async Task Verify_BulkTableUpdate_ShouldPassForAllTables()
    {
        var connection = CreateInMemoryDatabase(tableDefinitions);

        foreach (var (tableName, entityType) in tableDefinitions)
        {
            if (ignoreUpdateList.Any(ignore => entityType.Name.Contains(ignore)))
            {
                return;
            }
            var createUserID = 12;
            var resultId = await AddTestEntity(connection, tableName, entityType, createUserID);
            
            var updateUserRepositoryMock = new Mock<IUserRepository>();
            var updateUserID = 15;
            updateUserRepositoryMock.Setup(repo => repo.GetUserID()).ReturnsAsync(updateUserID);
            
            var updatedRepository = CreateRepositoryForEntity(entityType, connection, updateUserRepositoryMock.Object);
            
            var testEntity = CreateTestEntity(entityType);
            var idProperty = entityType.GetProperty("id");
            Assert.NotNull(idProperty);
            idProperty.SetValue(testEntity, resultId);

            PropertyInfo? nameProperty = null;
            try
            {
                nameProperty = entityType.GetProperty("Name");
                if (nameProperty != null && nameProperty.PropertyType == typeof(string))
                {
                    nameProperty.SetValue(testEntity, "Updated Value");
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to update {tableName} records: {e.Message}");
            }

            var updateMethod = updatedRepository.GetType().GetMethod("Update");
            Assert.True(updateMethod != null, $"{entityType.Name} is missing an Update method.");

            if (updateMethod.GetParameters().Length == 1)
            { 
                await (Task)updateMethod.Invoke(updatedRepository, new[] { testEntity });
            } 
            else if (updateMethod.GetParameters().Length == 2 && updateMethod.GetParameters()[1].ParameterType == typeof(SubscribtionContactType))
            {
                SubscribtionContactType? testContactType = new SubscribtionContactType
                { 
                    idTypeContact = Array.Empty<int>()
                };
                await (Task<bool>)updateMethod.Invoke(updatedRepository, new object[] { testEntity, testContactType });
            }
            
            var updatedRecord = await connection.QueryFirstOrDefaultAsync(
                $"SELECT * FROM {tableName} WHERE id = @id",
                new { id = resultId });

            Assert.NotNull(updatedRecord);
            var updatedDictionary = (IDictionary<string, object>)updatedRecord;

            try
            {
                if (nameProperty != null)
                {
                    Assert.Equal("Updated Value", updatedDictionary["name"].ToString());
                }

                Assert.True(updatedDictionary.ContainsKey("updated_at") && updatedDictionary["updated_at"] != null,
                    $"{entityType.Name} updated_at is null after update");
                Assert.True(
                    updatedDictionary.ContainsKey("updated_by") &&
                    int.Parse(updatedDictionary["updated_by"].ToString()) == updateUserID,
                    $"{entityType.Name} updated_by is invalid after update");


                Assert.True(updatedDictionary.ContainsKey("created_at") && updatedDictionary["created_at"] != null,
                    $"{entityType.Name} created_at should remain unchanged");
                Assert.True(
                    updatedDictionary.ContainsKey("created_by") &&
                    int.Parse(updatedDictionary["created_by"].ToString()) == createUserID,
                    $"{entityType.Name} created_by should remain unchanged");
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to update {tableName} records: {e.Message}");
            }
        }
    }

    private async Task<int> AddTestEntity(
        IDbConnection connection,
        string tableName,
        Type entityType,
        int createUserID)
    {
        var createUserRepositoryMock = new Mock<IUserRepository>();
        createUserRepositoryMock.Setup(repo => repo.GetUserID()).ReturnsAsync(createUserID);
        var testEntity = CreateTestEntity(entityType);
        var repository = CreateRepositoryForEntity(entityType, connection, createUserRepositoryMock.Object);
        var addMethod = repository.GetType().GetMethod("Add");
        Assert.True(addMethod != null, $"{entityType.Name} is missing an Add method.");
        int resultId = 0;
        if (addMethod.GetParameters().Length == 1)
        {
            resultId = await (Task<int>)addMethod.Invoke(repository, new[] { testEntity });
        }else if (addMethod.GetParameters().Length == 2 && addMethod.GetParameters()[1].ParameterType == typeof(SubscribtionContactType))
        {
            SubscribtionContactType? testContactType = new SubscribtionContactType
            { 
                idTypeContact = Array.Empty<int>()
            };
            resultId = await (Task<int>)addMethod.Invoke(repository, new object[] { testEntity, testContactType });
        }
        Assert.True(resultId > 0);
        var insertedRecord = await connection.QueryFirstOrDefaultAsync(
            $"SELECT * FROM {tableName} WHERE id = @id",
            new { id = resultId });
        Assert.NotNull(insertedRecord);
        return resultId;
    }

    private object CreateTestEntity(Type entityType)
    {
        var entity = Activator.CreateInstance(entityType);
        foreach (var property in entityType.GetProperties())
        {
            if (property.PropertyType == typeof(string))
            {
                property.SetValue(entity, "Test Value");
            }
            else if (property == typeof(int))
            {
                property.SetValue(entity, 1);
            }
        }

        return entity;
    }

    private object CreateRepositoryForEntity(Type entityType, IDbConnection connection,
        params object[] additionalDependencies)
    {
        var repositoryName = $"{entityType.Name}Repository";
        var repositoryType = Assembly.Load("Infrastructure").GetTypes()
            .FirstOrDefault(t => t.Name == repositoryName && t.IsClass && !t.IsAbstract);

        if (repositoryType == null)
            throw new InvalidOperationException($"Repository for entity {entityType.Name} not found.");

        var constructor = repositoryType.GetConstructors()
            .FirstOrDefault(c => c.GetParameters()
                .All(p =>
                    p.ParameterType == typeof(IDbConnection) ||
                    p.ParameterType == typeof(IHostEnvironment) ||
                    additionalDependencies.Any(d => p.ParameterType.IsInstanceOfType(d)) ||
                    p.IsOptional));

        var parameters = constructor?.GetParameters()
            .Select(p =>
            {
                if (p.ParameterType == typeof(IDbConnection))
                    return connection;
                
                if (p.ParameterType == typeof(IHostEnvironment))
                {
                    var existingHostEnv = additionalDependencies.FirstOrDefault(d => d is IHostEnvironment);
                    if (existingHostEnv != null)
                        return existingHostEnv;
                    
                    var mockHostEnvironment = new Mock<IHostEnvironment>();
                    mockHostEnvironment.Setup(env => env.EnvironmentName).Returns("Development");
                    mockHostEnvironment.Setup(env => env.ContentRootPath).Returns("/fake/path");
                    return mockHostEnvironment.Object;
                }
                
                var matchingDependency = additionalDependencies.FirstOrDefault(d => p.ParameterType.IsInstanceOfType(d));
                if (matchingDependency != null)
                    return matchingDependency;

                if (p.IsOptional)
                    return p.DefaultValue;

                throw new InvalidOperationException(
                    $"Missing dependency for parameter '{p.Name}' of type '{p.ParameterType.FullName}' " +
                    $"in repository '{repositoryType.Name}'.");
            })
            .ToArray();
        
        return constructor.Invoke(parameters);
    }

    private IDbConnection CreateInMemoryDatabase(List<(string TableName, Type EntityType)> tableDefinitions)
    {
        Batteries.Init();
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        foreach (var (tableName, entityType) in tableDefinitions)
        {
            try
            {
                var createTableSql = GenerateCreateTableSql(tableName, entityType);
                connection.Execute(createTableSql);
            }
            catch (Exception e)
            {
                throw new Exception($"{e.Message}");
            }
        }


        return connection;
    }

    private string GenerateCreateTableSql(string tableName, Type entityType)
    {
        var columns = new List<string>();
        foreach (var property in entityType.GetProperties())
        {
            var columnName = $"[{property.Name.ToLower()}]";
            var columnType = GetSqlType(property.PropertyType);
            var isPrimaryKey = property.Name.Equals("Id", StringComparison.OrdinalIgnoreCase);

            if (isPrimaryKey)
            {
                columns.Add($"{columnName} {columnType} PRIMARY KEY AUTOINCREMENT");
            }
            else
            {
                columns.Add($"{columnName} {columnType}");
            }
        }

        var escapedTableName = $"[{tableName}]";
        var createTableSql = $"CREATE TABLE {escapedTableName} (\n    {string.Join(",\n    ", columns)}\n);";
        return createTableSql;
    }

    private string GetSqlType(Type propertyType)
    {
        if (propertyType == typeof(int) || propertyType == typeof(long))
        {
            return "INTEGER";
        }

        if (propertyType == typeof(string))
        {
            return "TEXT";
        }

        if (propertyType == typeof(DateTime))
        {
            return "DATETIME";
        }

        if (propertyType == typeof(bool))
        {
            return "INTEGER";
        }

        if (propertyType == typeof(double) || propertyType == typeof(float))
        {
            return "REAL";
        }

        return "TEXT";
    }
}