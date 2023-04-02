import { FormGroup, FormControl } from "@angular/forms";

export default class ValidateForm{
    static validateAllFormField(formGroup: FormGroup)
    {
        Object.keys(formGroup.controls).forEach(x => {
            const control = formGroup.get(x);
            if (control instanceof FormControl)
            {
            control.markAsDirty({onlySelf: true});
            }
            else if (control instanceof FormGroup)
            {
            this.validateAllFormField(control);
            }
        })
    }
}