import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { TemperatatureVM } from 'src/app/models/temperatatureVM';
import { DataAccessService } from 'src/app/services/data-access.service';

@Component({
  selector: 'app-temperature',
  templateUrl: './temperature.component.html',
  styleUrls: ['./temperature.component.scss'],
})
export class TemperatureComponent implements OnInit {
  temperature: TemperatatureVM = {
    from: 'C'.charCodeAt(0),
    value: 30,
    to: 'F'.charCodeAt(0),
  };

  result: TemperatatureVM = {
    from: 'C'.charCodeAt(0),
    value: 60,
    to: 'F'.charCodeAt(0),
  };

  resultOption = 'F';

  tmpOpts = [
    { optTxt: 'Celsius -> Fahrenheit', val: 'CF' },
    { optTxt: 'Celsius -> Kelvin', val: 'CK' },
    { optTxt: 'Fahrenheit -> Celsius', val: 'FC' },
    { optTxt: 'Fahrenheit -> Kelvin', val: 'FK' },
    { optTxt: 'Kelvin -> Fahrenheit', val: 'KF' },
    { optTxt: 'Kelvin -> Celsius', val: 'KC' },
  ];

  form: FormGroup;

  constructor(private dataAccess: DataAccessService, formb: FormBuilder) {
    this.form = formb.group({
      tempOptions: ['FC', [Validators.required]],
      inputValue: [30, [Validators.required]],
    });
  }

  ngOnInit(): void {}

  reset(): void {
    this.temperature = {
      from: 'C'.charCodeAt(0),
      value: 30,
      to: 'F'.charCodeAt(0),
    };

    this.result = {
      from: 'C'.charCodeAt(0),
      value: 60,
      to: 'F'.charCodeAt(0),
      message: null,
    };
    this.resultOption = String.fromCharCode(this.result.to);
  }

  convertTemperature(): void {
    this.temperature.value = this.f.inputValue.value;
    console.log(this.temperature);
    this.dataAccess.convertTemperature(this.temperature).subscribe(
      (tmp) => {
        this.result = tmp;
        this.resultOption = String.fromCharCode(tmp.to);
      },
      (err) => this.reset()
    );
  }

  tempChanged($event): void {
    if (this.f.tempOptions.value && this.f.tempOptions.value.length === 2) {
      this.temperature.from = this.f.tempOptions.value.charCodeAt(0);
      this.temperature.to = this.f.tempOptions.value.charCodeAt(1);
      this.resultOption = '_';
      this.result.value = 0;
    }
    // console.log('tmp', this.f.tempOptions.value, this.temperature);
  }
  // tslint:disable-next-line:typedef
  get f() {
    return this.form.controls;
  }
}
