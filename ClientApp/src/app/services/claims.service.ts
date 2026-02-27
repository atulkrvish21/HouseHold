import { Injectable } from '@angular/core';
import { JwtService } from './jwt.service';

@Injectable({
  providedIn: 'root'
})
export class ClaimsService {
  constructor(private jwtService: JwtService) { }

  getClaims(token: string): any {
    return this.jwtService.decodeToken(token);
  }
  getNameClaim(token: string): string | null {
    const claims = this.getClaims(token);
    return claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] || claims.name || claims.nameid || claims.sub || null;
  }

  getGivenClaim(token: string): string | null {
    const claims = this.getClaims(token);
    return claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"] || claims.givenname || claims.nameid || claims.sub || null;
  }
  getRoleClaim(token: string): string | null {
    const claims = this.getClaims(token);
    console.log("Claims object:", claims);

    // Specifically look for the role claim from your C# backend
    return claims['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || null;
  }
  getEmailClaim(token: string): string | null {
    const claims = this.getClaims(token);
    console.log("Claims object:", claims);

    // Specifically look for the role claim from your C# backend
    return claims['http://schemas.microsoft.com/ws/2008/06/identity/claims/email'] || null;
  }
}
