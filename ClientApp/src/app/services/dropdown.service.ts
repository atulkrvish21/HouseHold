
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DropdownService {


  constructor(private http: HttpClient) { }

  getSourceOfConnection(): Observable<any[]> {
    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}` // Include the token in the Authorization header
    });

    return this.http.get<any[]>(`${environment.apiUrl}/SourceOfConnection`, { headers }); // Replace with your API URL
  }


  getBank(): Observable<any[]> {
    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}` // Include the token in the Authorization header
    });

    return this.http.get<any[]>(`${environment.apiUrl}/bankList`, { headers }); // Replace with your API URL
  }

  getBankifsCode(bankName: string): Observable<any[]> {
    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}` // Include the token in the Authorization header
    });
    const params = {
      bankName: bankName
     
    };
     return this.http.get<any[]>(`${environment.apiUrl}/BankIfsCode/${bankName}`, { headers }); // Replace with your API URL
  }
  getDistrict(): Observable<any[]> {
    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}` // Include the token in the Authorization header
    });

    return this.http.get<any[]>(`${environment.apiUrl}/DistrictList`, { headers }); // Replace with your API URL
  }
getAllBlock(): Observable<any[]> {
    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}` // Include the token in the Authorization header
    });
    
    return this.http.get<any[]>(`${environment.apiUrl}/BlockList`, { headers }); // Replace with your API URL
  }
  getBlock(districtCode:number): Observable<any[]> {
    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}` // Include the token in the Authorization header
    });
    const params = {
      districtCode: districtCode
     
    };
    return this.http.get<any[]>(`${environment.apiUrl}/BlockList/${districtCode}`, { headers }); // Replace with your API URL
  }

getAllPanchayat(): Observable<any[]> {
    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}` // Include the token in the Authorization header
    });
    
    return this.http.get<any[]>(`${environment.apiUrl}/panchayatList`, {  headers }); // Replace with your API URL
  }

  getPanchayat(blockCode: number): Observable<any[]> {
    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}` // Include the token in the Authorization header
    });
    const params = {
      blockCode: blockCode

    };
    return this.http.get<any[]>(`${environment.apiUrl}/panchayatList/${blockCode}`, {  headers }); // Replace with your API URL
  }


  getAllVillageCode(): Observable<any[]> {
    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}` // Include the token in the Authorization header
    });
    
    return this.http.get<any[]>(`${environment.apiUrl}/villageList`, { headers }); // Replace with your API URL
  }

  getVillageCode(panchayatCode: number): Observable<any[]> {
    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}` // Include the token in the Authorization header
    });
    const params = {
      panchayatCode: panchayatCode

    };
    return this.http.get<any[]>(`${environment.apiUrl}/villageList/${panchayatCode}`, { headers }); // Replace with your API URL
  }


  getidProofMaster(): Observable<any[]> {
    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}` // Include the token in the Authorization header
    });

    return this.http.get<any[]>(`${environment.apiUrl}/idProof`, { headers }); // Replace with your API URL
  }
}
