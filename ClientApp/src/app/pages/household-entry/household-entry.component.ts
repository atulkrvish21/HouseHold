import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Household, createEmptyHousehold } from '../../model/household.model';
import { environment } from 'src/environments/environment';
import { DropdownService } from 'src/app/services/dropdown.service';
import { NotificationService } from 'src/app/services/notification.service';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-household-entry',
  templateUrl: './household-entry.component.html',
  styleUrls: ['./household-entry.component.css']
})
export class HouseholdEntryComponent implements OnInit {

editMode = false; // Track if we are editing
  editId: string | null = null;
  existingPhotoUrl: string | null = null; // To show current photo

  formData: Household = createEmptyHousehold();
  selectedPhoto: File | null = null;
  isSubmitting = false;
  message = '';

  // Dropdown Options (Mock Data)
  headgenderOptions = ['Male', 'Female', 'Other'];
  genderOptions = ['Male', 'Female ', 'Other'];
  socialCategories = ['SC', 'ST', 'OBC', 'General'];
  relationships = ['Spouse', 'Daughter', 'Daughter-In-Law', 'Sister', 'Mother', 'Self'];
  maritalStatuses = ['Married', 'Widow', 'Unmarried', 'Divorced'];
  waterSources = ['Tap Water', 'Tube Well', 'Well', 'River/Pond'];
  occupations = ['Agriculture', 'Daily Wage Labour', 'Salaried', 'Business', 'Other'];
  kishanScheme = ['PM Kishan', 'CM Kishan', 'Both'];
  FRAClaimStatus = ['Not a FRA claimant', 'FRA Claimant'];
  PrivateHolding = ['Landless', '0- 0.5 Acr', '0.5- 1 Acr', '1 - 2.5 Acr', 'more than 2.5 Acr']; 
  LivestockActivity = ['Poultry','Goatery','Dairy','Others'];
  RespondantIdentity = ['Migrant Person himself','Other Adult family member','Village Head / Ward Member','Neighbor','Head of the Household'];  
  SourceOfIrrigation = ['Major', 'Medium', 'Minor', 'Lift Irrigation', 'Check Dam', 'Canal', 'Bore well', 'Dug well', 'Farm pond', 'Others'];  
  StateListMaster = ['ANDAMAN AND NICOBAR','ANDHRA PRADESH','ARUNACHAL PRADESH','ASSAM','BIHAR','CHANDIGARH','CHHATTISGARH','DADAR AND NAGAR HAVELI','DAMAN AND DIU','DELHI','GOA','GUJARAT','HARYANA','HIMACHAL PRADESH','JAMMU AND KASHMIR','JHARKHAND','KARNATAKA','KERALA','LAKSHADWEEP','MADHYA PRADESH','MAHARASTRA','MANIPUR','MEGHALAYA','MIZORAM','NAGALAND','ORISSA','PUDUCHERRY','PUNJAB','RAJASTHAN','SIKKIM','TAMIL NADU', 'TELANGANA', 'TRIPURA', 'UTTAR PRADESH', 'UTTARAKHAND', 'WEST BENGAL'];
  NatureofEngagement = ['Brick Kiln', 'Construction Labour', 'Agri Labour', 'Mason', 'Domestic Support', 'Manufacturing', 'Service Sector (Hotel, Hospital, Security)', 'Other'];
  PeriodOfMigration = ['1- 3months', '4-6 months', '7-12 months'];
  SkillDevelopment = ['DDUGKY', 'RSETI', 'Other', 'None'];
  ddldistrictList: any[] = [];
  districtselectedOption: string = '';
  ddlBlockList: any[] = [];
  blockselectedOption: string = '';
  ddlPanchayatList: any[] = [];
  panchayatselectedOption: string = '';
  ddlVillageList: any[] = [];
  villageselectedOption: string = '';
  ddlBankList: any[] = [];
  bankselectedOption: string = '';
  ddlBankiscodeList: any[] = [];
  bankifscodeselectedOption: string = '';  

  validationErrors: string[] = [];
  constructor(private http: HttpClient, private dropdownService: DropdownService, private notificationService: NotificationService, private route: ActivatedRoute) {}

  ngOnInit() {
    this.captureLocation();
     this.dropdownService.getDistrict().subscribe(
      (data) => {
        this.ddldistrictList = data;
      },
      (error) => {
        console.error('Error fetching dropdown options:', error);
      }
    );

     this.dropdownService.getBank().subscribe(
      (data) => {
        this.ddlBankList = data;
      },
      (error) => {
        console.error('Error fetching dropdown options:', error);
      }
    );

    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.editMode = true;
        this.editId = id;
        this.loadDataForEdit(id);
      }
    });
  }
 loadDataForEdit(id: string) {

    this.http.get<any>(`${environment.apiUrl}/Household/${id}`).subscribe({
      next: (res) => {
        // Map API response to your formData model
        this.formData = res; 
        const distCode = this.formData.householdBasicProfile.district;
     const blockCode = this.formData.householdBasicProfile.block;
     const panchayatCode = this.formData.householdBasicProfile.gramPanchayat;
     const villageCode = this.formData.householdBasicProfile.revenueVillage;
      const bankName = this.formData.householdBasicProfile.bankName;
      
      // Call your service manually (Don't just call ondistrictChange if it clears variables)
     
      
        // Handle Photo Preview (since file inputs can't be pre-filled)
        if (res.householdMigrationStatus?.respondentPhotoPathOrUrl) {
           this.existingPhotoUrl = res.householdMigrationStatus.respondentPhotoPathOrUrl;
        }
this.dropdownService.getBlock(Number(distCode)).subscribe(blocks => {
        this.ddlBlockList = blocks;
});

this.dropdownService.getPanchayat(Number(blockCode)).subscribe(panchayats => {
        this.ddlPanchayatList = panchayats;
});
this.dropdownService.getVillageCode(Number(panchayatCode)).subscribe(villages => {
        this.ddlVillageList = villages;
});

this.dropdownService.getBank().subscribe(banks => {
        this.ddlBankList = banks;
});
this.dropdownService.getBankifsCode(String(bankName)).subscribe(bankIfsc => {
        this.ddlBankiscodeList = bankIfsc;
});

      },
      error: (err) => console.error('Failed to load data', err)
    });
  }
  ondistrictChange(districtCode: number): void {
    this.dropdownService.getBlock(districtCode).subscribe(
      (data) => {
        this.ddlBlockList = data;
      },
      (error) => {
        console.error('Error fetching dropdown options:', error);
      }
    );
  }

   onbankChange(bankName: string): void {
    this.dropdownService.getBankifsCode(bankName).subscribe(
      (data) => {
        this.ddlBankiscodeList = data;
      },
      (error) => {
        console.error('Error fetching dropdown options:', error);
      }
    );
  }

  onblockChange(blockCode: number): void {
    this.dropdownService.getPanchayat(blockCode).subscribe(
      (data) => {
        this.ddlPanchayatList = data;
      },
      (error) => {
        console.error('Error fetching dropdown options:', error);
      }
    );
  }

  onpanchayatChange(panchayatCode: number): void {
    this.dropdownService.getVillageCode(panchayatCode).subscribe(
      (data) => {
        this.ddlVillageList = data;
      },
      (error) => {
        console.error('Error fetching dropdown options:', error);
      }
    );
  }
  // Auto-capture Geolocation
  captureLocation() {
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition((position) => {
        this.formData.householdBasicProfile.geoLocation = 
          `${position.coords.latitude},${position.coords.longitude}`;
      });
    }
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedPhoto = file;
    }
  }

  addFamilyMember() {
    this.formData.householdFamilyMember.push({
      name: '', age: null, gender: '', educationalQualification: '',
      migratedInLast3Years: false, destinationState: '', sectorOfEngagementDuringMigration: '',
      periodOfMigration: '', monthlyRemittanceDuringMigration: null, interestInSkillDevelopment: ''
    });
  }

  removeFamilyMember(index: number) {
    this.formData.householdFamilyMember.splice(index, 1);
  }

  submitForm() {
    this.isSubmitting = true;
    this.message = '';
    this.validationErrors = []; // Reset errors
    const formD = JSON.stringify(this.formData);
    console.log(formD);
    const uploadData = new FormData();
    uploadData.append('householdJson', formD);
    
    if (this.selectedPhoto) {
      uploadData.append('respondentPhoto', this.selectedPhoto, this.selectedPhoto.name);
    }
    var token = localStorage.getItem('token') || '';
const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    
if (this.editMode && this.editId) {
      // --- UPDATE REQUEST (PUT) ---
      this.http.post(`${environment.apiUrl}/Household/${this.editId}`, uploadData)
        .subscribe({
        next: (res: any) => {
          if(res.success)
          {
          this.message = "Household ID:" + res.uniqueId || 'Household data submitted successfully!';
          this.isSubmitting = false;
          this.formData = createEmptyHousehold();
          this.selectedPhoto = null;
          this.captureLocation(); // Reset location for next entry
          window.scrollTo(0,0);
          }
          else {
            this.isSubmitting = false;
             this.message = 'Submission Failed. Please correct the errors below.';
          if (res.errors) {
            this.extractErrors(res.errors);
          }
          }
        },
        error: (err) => {
          console.error(err);
        this.isSubmitting = false;
        
        // CASE B: Server returns 400 Bad Request (Standard for Validation Errors)
        // The error object is usually inside err.error.errors
        if (err.status === 400 && err.error?.errors) {
            this.message = 'Validation Failed. Please check the form.';
            this.extractErrors(err.error.errors);
        } else {
            // Generic fallback
            this.message = 'Error: ' + (err.error?.title || err.statusText || 'Server Error');
        }
        window.scrollTo(0, 0);
        }
      });
    }

else {
    this.http.post(`${environment.apiUrl}/Household`, uploadData, { headers: headers })
      .subscribe({
        next: (res: any) => {
          if(res.success)
          {
          this.message = "Household ID:" + res.uniqueId || 'Household data submitted successfully!';
          this.isSubmitting = false;
          this.formData = createEmptyHousehold();
          this.selectedPhoto = null;
          this.captureLocation(); // Reset location for next entry
          window.scrollTo(0,0);
          }
          else {
            this.isSubmitting = false;
             this.message = 'Submission Failed. Please correct the errors below.';
          if (res.errors) {
            this.extractErrors(res.errors);
          }
          }
        },
        error: (err) => {
          console.error(err);
        this.isSubmitting = false;
        
        // CASE B: Server returns 400 Bad Request (Standard for Validation Errors)
        // The error object is usually inside err.error.errors
        if (err.status === 400 && err.error?.errors) {
            this.message = 'Validation Failed. Please check the form.';
            this.extractErrors(err.error.errors);
        } else {
            // Generic fallback
            this.message = 'Error: ' + (err.error?.title || err.statusText || 'Server Error');
        }
        window.scrollTo(0, 0);
        }
      });
    }
  }


  extractErrors(errorsObj: any) {
  // 1. Get all the arrays of error messages (values of the object)
  const values = Object.values(errorsObj);
  
  // 2. Flatten them into a single array of strings
  // Example: [['Err1'], ['Err2', 'Err3']] -> ['Err1', 'Err2', 'Err3']
this.validationErrors = values.reduce((acc: any[], curr: any) => acc.concat(curr), []) as string[];
}
}