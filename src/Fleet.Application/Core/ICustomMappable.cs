using Mapster;

namespace Fleet.Application.Core;

public interface ICustomMappable<TModel, TTarget>
{
    public void ConfigureMapper(TypeAdapterSetter<TModel, TTarget> config);
    public void ConfigureInvertMapper(TypeAdapterSetter<TTarget, TModel> config);
}