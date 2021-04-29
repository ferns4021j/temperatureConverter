import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { TemperatatureVM } from '../models/temperatatureVM';

@Injectable({
  providedIn: 'root',
})
export class DataAccessService {
  constructor(private http: HttpClient) {}

  convertTemperature(
    temperature: TemperatatureVM
  ): Observable<TemperatatureVM> {
    const header = new HttpHeaders({
      'access-control-allow-origin': 'http://localhost:4200',
      'content-type': 'application/json',
    });
    let param = new HttpParams();
    param = param.append('from', temperature.from.toString());
    param = param.append('value', temperature.value.toString());
    param = param.append('to', temperature.to.toString());
    return this.http.get<TemperatatureVM>(
      'https://localhost:44363/api/Temp/convert',
      { headers: header, params: param }
    );
  }

  convertTemperature1(
    temperature: TemperatatureVM
  ): Observable<TemperatatureVM> {
    return of({
      from: temperature.from,
      value: temperature.value * 2.0,
      to: temperature.to,
    });
  }
}
