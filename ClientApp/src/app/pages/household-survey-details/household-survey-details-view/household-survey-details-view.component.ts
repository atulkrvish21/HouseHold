import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { environment } from 'src/environments/environment';
import { HouseholdFamilyMember } from 'src/app/model/household.model';
import { HouseholdSurveyRecord } from 'src/app/services/household-survey.service';

@Component({
  selector: 'app-household-survey-details-view',
  templateUrl: './household-survey-details-view.component.html',
  styleUrls: ['./household-survey-details-view.component.scss']
})
export class HouseholdSurveyDetailsViewComponent {
  readonly baseUrl = environment.baseUrl.replace(/\/$/, '');

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: HouseholdSurveyRecord,
    private dialogRef: MatDialogRef<HouseholdSurveyDetailsViewComponent>
  ) {}

  close(): void {
    this.dialogRef.close();
  }

  formatBoolean(value: boolean | null | undefined): string {
    if (value === null || value === undefined) {
      return 'N/A';
    }

    return value ? 'Yes' : 'No';
  }

  formatValue(value: string | number | null | undefined, fallback = 'N/A'): string | number {
    if (value === null || value === undefined || value === '') {
      return fallback;
    }

    return value;
  }

  formatMemberBoolean(value: boolean | null | undefined): string {
    if (value === null || value === undefined) {
      return '-';
    }

    return value ? 'Yes' : 'No';
  }

  resolveAssetUrl(path: string | null | undefined): string | null {
    if (!path) {
      return null;
    }

    if (path.startsWith('http://') || path.startsWith('https://')) {
      return path;
    }

    return `${this.baseUrl}/${path.replace(/^\/+/, '')}`;
  }

  get familyMembers(): HouseholdFamilyMember[] {
    return this.data.householdFamilyMember ?? [];
  }
}
