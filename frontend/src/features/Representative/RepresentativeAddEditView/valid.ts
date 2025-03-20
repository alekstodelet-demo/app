import * as yup from "yup";

export const schema = yup.object().shape({
  last_name: yup.string().required("Обязательное поле"),
  first_name: yup.string().required("Обязательное поле"),
  type_id: yup.number().test({
    name: 'type_id',
    message: 'Выберите тип представителя',
    test: value => value > 0,
  }),
});

export const validateField = async (name: string, value: any) => {
  try {
    const schemas = yup.object().shape({
      [name]: schema.fields[name],
    });
    await schemas.validate({ [name]: value }, { abortEarly: false });
    return { isValid: true, error: "" };
  } catch (validationError) {
    return { isValid: false, error: validationError.errors[0] };
  }
};

export const validate = async (data: any) => {
  try {
    await schema.validate(data, { abortEarly: false });
    return { isValid: true, errors: {} };
  } catch (validationErrors) {
    let errors: any = {};
    validationErrors.inner.forEach((error: any) => {
      errors[error.path] = error.message;
    });
    return { isValid: false, errors };
  }
};