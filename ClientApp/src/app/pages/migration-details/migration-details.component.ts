import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { finalize } from 'rxjs';
import { MigrationSurvey } from 'src/app/model/migration-survey.model';
import { MigrationSurveyService } from 'src/app/services/migration-survey.service';
import { MigrationDetailsViewComponent } from './migration-details-view/migration-details-view.component';

@Component({
  selector: 'app-migration-details',
  templateUrl: './migration-details.component.html',
  styleUrls: ['./migration-details.component.scss'],
 
})
export class MigrationDetailsComponent implements OnInit {
  displayedColumns: string[] = [
    'respondentName',
    'identityRole',
    'district',
    'block',
    'totalPersonsInMigration',
    'action'
  ];
  dataSource = new MatTableDataSource<MigrationSurvey>([]);
  loading = false;
  errorMessage = '';

  @ViewChild(MatPaginator)
  set paginator(paginator: MatPaginator | undefined) {
    if (paginator) {
      this.dataSource.paginator = paginator;
    }
  }

  constructor(
    private migrationSurveyService: MigrationSurveyService,
    private dialog: MatDialog
  ) {
    this.dataSource.filterPredicate = (record: MigrationSurvey, filter: string) => {
      const term = filter.trim().toLowerCase();
      const haystack = [
        record.respondentName,
        record.identityRole,
        record.district,
        record.block,
        record.gramPanchayat,
        record.revenueVillage,
        record.enumeratorName,
        record.respondentMobile
      ]
        .filter((value): value is string => !!value)
        .join(' ')
        .toLowerCase();

      return haystack.includes(term);
    };
  }

  ngOnInit(): void {
    this.loadMigrationSurveys();
  }

  applyFilter(value: string): void {
    this.dataSource.filter = value.trim().toLowerCase();
  }

  loadMigrationSurveys(): void {
    this.loading = true;
    this.errorMessage = '';

    this.migrationSurveyService.getMigrationSurveys()
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe({
        next: (records) => {
          this.dataSource.data = records ?? [];
        },
        error: () => {
          this.errorMessage = 'Unable to load migration survey records right now. Please try again.';
          this.dataSource.data = [];
        }
      });
  }

  openDetails(record: MigrationSurvey): void {
    this.dialog.open(MigrationDetailsViewComponent, {
      autoFocus: false,
      data: record,
      maxWidth: '1100px',
      panelClass: 'migration-details-dialog',
      width: '95vw'
    });
  }
}
