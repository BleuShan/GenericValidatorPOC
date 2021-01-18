namespace GenericValidatorPOC.Core
{
  public interface IValidator<TargetT> where TargetT : new()
  {
    void Validate(TargetT target);
  }
}