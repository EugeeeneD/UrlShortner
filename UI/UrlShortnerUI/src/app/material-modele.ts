import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';

@NgModule({
    exports:[
        MatTableModule,
        MatPaginatorModule,
        MatButtonModule
    ]
})

export class MaterialModule{

}