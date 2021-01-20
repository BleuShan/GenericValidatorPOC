namespace GenericValidatorPOC.Core {
  public interface IValidator<TArgetT>
    where TArgetT : new() {
    void Validate(TArgetT target);
  }
}