import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { finalize } from 'rxjs';
import { HouseholdSurveyRecord, HouseholdSurveyService } from 'src/app/services/household-survey.service';
import { HouseholdSurveyDetailsViewComponent } from './household-survey-details-view/household-survey-details-view.component';

@Component({
  selector: 'app-household-survey-details',
  templateUrl: './household-survey-details.component.html',
  styleUrls: ['./household-survey-details.component.scss']
})
export class HouseholdSurveyDetailsComponent implements OnInit, AfterViewInit {
  displayedColumns: string[] = [
    'entryDate',
    'headName',
    'location',
    'category',
    'familyMembers',
    'action'
  ];
  dataSource = new MatTableDataSource<HouseholdSurveyRecord>([]);
  loading = false;
  errorMessage = '';

  @ViewChild(MatPaginator) paginator?: MatPaginator;

  constructor(
    private householdSurveyService: HouseholdSurveyService,
    private dialog: MatDialog
  ) {
    this.dataSource.filterPredicate = (record: HouseholdSurveyRecord, filter: string) => {
      const term = filter.trim().toLowerCase();
      const basic = record.householdBasicProfile;
      const haystack = [
        basic?.uniqueId,
        basic?.headOfTheHouseholdNameAsPerAadhar,
        basic?.district,
        basic?.block,
        basic?.gramPanchayat,
        basic?.revenueVillage,
        basic?.socialCategory,
        basic?.entryBy,
        basic?.entryDate
      ]
        .filter((value): value is string => !!value)
        .join(' ')
        .toLowerCase();

      return haystack.includes(term);
    };
  }

  ngOnInit(): void {
    this.loadHouseholds();
  }

  ngAfterViewInit(): void {
    this.attachPaginator();
  }

  loadHouseholds(): void {
    this.loading = true;
    this.errorMessage = '';

    this.householdSurveyService.getHouseholds()
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe({
        next: (records) => {
          this.dataSource.data = records ?? [];
          this.attachPaginator();
        },
        error: () => {
          this.errorMessage = 'Unable to load household survey records right now. Please try again.';
          this.dataSource.data = [];
        }
      });
  }

  applyFilter(value: string): void {
    this.dataSource.filter = value.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  openDetails(record: HouseholdSurveyRecord): void {
    this.dialog.open(HouseholdSurveyDetailsViewComponent, {
      autoFocus: false,
      data: record,
      maxWidth: '1200px',
      panelClass: 'household-survey-details-dialog',
      width: '95vw'
    });
  }

  private attachPaginator(): void {
    if (this.paginator) {
      this.dataSource.paginator = this.paginator;
    }
  }
}
