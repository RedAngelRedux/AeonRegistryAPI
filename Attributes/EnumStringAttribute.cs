namespace AeonRegistryAPI.Attributes;

public class EnumStringAttribute : Attribute
{
    public Type EnumType { get; }
    
    public EnumStringAttribute(Type enumType)
    {
        EnumType = enumType;
    }
}
