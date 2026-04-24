import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  HouseholdBasicProfile,
  HouseholdEntitlement,
  HouseholdFamilyMember,
  HouseholdMigrationStatus,
  HouseholdOccupationAndLand
} from '../model/household.model';
import { environment } from 'src/environments/environment';

export interface HouseholdSurveyRecord {
  householdBasicProfile: HouseholdBasicProfile & {
    uniqueId?: string | null;
    nearestLandmark?: string | null;
    photoPath?: string | null;
    entryBy?: string | null;
    sstatus?: number | null;
    entryDate?: string | null;
    approvedDate?: string | null;
    approvedBy?: string | null;
    reason?: string | null;
  };
  householdEntitlement: HouseholdEntitlement;
  householdMigrationStatus: Omit<HouseholdMigrationStatus, 'minorChildrenAccompaniedMigration'> & {
    hasFamilyMemberMigratedLast3Years?: boolean | null;
    takenAdvanceForMigrationFromMiddleman?: boolean | null;
    minorChildrenAccompaniedMigration?: number | null;
    womenMembersMigrated?: boolean | null;
    familyContactMobileNo?: string | null;
    respondentIdentity?: string | null;
    respondentPhotoPathOrUrl?: string | null;
    entryDate?: string | null;
  };
  householdOccupationAndLand: HouseholdOccupationAndLand;
  householdFamilyMember: HouseholdFamilyMember[];
}

@Injectable({
  providedIn: 'root'
})
export class HouseholdSurveyService {
  private readonly householdUrl = `${environment.apiUrl}/Household`;

  constructor(private http: HttpClient) {}

  getHouseholds(): Observable<HouseholdSurveyRecord[]> {
    return this.http.get<HouseholdSurveyRecord[]>(this.householdUrl);
  }

  getHouseholdById(uniqueId: string): Observable<HouseholdSurveyRecord> {
    return this.http.get<HouseholdSurveyRecord>(`${this.householdUrl}/${uniqueId}`);
  }
}
