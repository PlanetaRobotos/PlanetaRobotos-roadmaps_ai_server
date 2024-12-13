using FluentValidation;
using Mapster;

namespace Fleet.Application.Core;

public abstract class ModelBase<TModel, TEntity> : IRegister
    where TModel : class, new()
    where TEntity : class, new()
{
    public TEntity ToEntity()
    {
        return this.Adapt<TEntity>();
    }

    public TEntity ToEntity(TEntity entity)
    {
        return (this as TModel).Adapt(entity);
    }

    public static TModel FromEntity(TEntity entity)
    {
        return entity.Adapt<TModel>();
    }


    private TypeAdapterConfig Config { get; set; }

    protected virtual void ConfigureMapper() { }

    protected abstract void ConfigureValidator(InlineValidator<TModel> validator);

    protected TypeAdapterSetter<TModel, TEntity> SetCustomMappings() =>
        Config.ForType<TModel, TEntity>();

    protected TypeAdapterSetter<TEntity, TModel> SetCustomMappingsInverse() =>
        Config.ForType<TEntity, TModel>();

    public void Register(TypeAdapterConfig config)
    {
        Config = config;
        ConfigureMapper();
    }


    public InlineValidator<TModel> Validator
    {
        get
        {
            var validator = new InlineValidator<TModel>();
            ConfigureValidator(validator);
            return validator;
        }
    }
}