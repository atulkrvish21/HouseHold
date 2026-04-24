import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { environment } from 'src/environments/environment';
import { MigrationSurvey } from 'src/app/model/migration-survey.model';

@Component({
  selector: 'app-migration-details-view',
  templateUrl: './migration-details-view.component.html',
  styleUrls: ['./migration-details-view.component.scss']
})
export class MigrationDetailsViewComponent {
  readonly baseUrl = environment.baseUrl.replace(/\/$/, '');

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: MigrationSurvey,
    private dialogRef: MatDialogRef<MigrationDetailsViewComponent>
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

  formatDistance(value: number | null | undefined): string {
    if (value === null || value === undefined) {
      return 'N/A';
    }

    return `${value} km`;
  }

  resolvePhotoUrl(path: string | null | undefined): string | null {
    if (!path) {
      return null;
    }

    if (path.startsWith('http://') || path.startsWith('https://')) {
      return path;
    }

    return `${this.baseUrl}/${path.replace(/^\/+/, '')}`;
  }
}
