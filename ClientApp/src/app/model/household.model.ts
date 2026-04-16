export interface Household {
  householdBasicProfile: HouseholdBasicProfile;
  householdEntitlement: HouseholdEntitlement;
  householdMigrationStatus: HouseholdMigrationStatus;
  householdOccupationAndLand: HouseholdOccupationAndLand;
  householdFamilyMember: HouseholdFamilyMember[];
}

export interface HouseholdBasicProfile {
  // Location
  district: string;
  block: string;
  gramPanchayat: string;
  revenueVillage: string;
  hamlet: string;

  // Head of Household
  headOfTheHouseholdNameAsPerAadhar: string;
  headOfTheHouseholdGender: string;
  aadharNo: string;
  socialCategory: string;

  // Bank
  bankAccountNumber: string;
  bankName: string;
  ifscCodeOrBranch: string;

  // Women Member
  womenMemberName: string;
  womenMemberAge: number | null;
  womenMemberMaritalStatus: string;
  womenMemberRelationshipWithHead: string;
  isWomenCoveredUnderSHG: boolean;
  isWomenCoveredUnderSubhadraYojana: boolean;

  // Household Stats
  totalFamilyMembers: number | null;
  hasRationCard: boolean;
  rationCardNumber: string;
  drinkingWaterSource: string;
  hasUjjwalaLPGConnection: boolean;
  hasLabourCard: boolean;
  isCoveredUnderNSKY: boolean;
  geoLocation: string;
}

export interface HouseholdEntitlement {
  kishanSchemeCoverage: string;
  hasRuralHousingSchemeHouse: boolean;
  hasIndividualHouseholdLatrine: boolean;
  hasElectricityConnection: boolean;
  hasMGNREGSJobCard: boolean;
  fullJobCardNumber: string;
  hasJanDhanYojanaAccount: boolean;
  isCoveredUnderAyushmanBharat: boolean;
  isEnrolledUnderShramYogiMaandhan: boolean;
  isCoveredUnderPMJJBY: boolean;
  isCoveredUnderPMSBY: boolean;
  atalPension: boolean;
  oldagePension: boolean;
  widowPension: boolean;
  disabilityPension: boolean;
}

export interface HouseholdMigrationStatus {
  hasFamilyMemberMigratedLast3Years: boolean;
  takenAdvanceForMigrationFromMiddleman: boolean;
  minorChildrenAccompaniedMigration: boolean;
  womenMembersMigrated: boolean;
  familyContactMobileNo: string;
  respondentIdentity: string;
  // Note: Photo is handled separately as a File object
}

export interface HouseholdOccupationAndLand {
  primaryOccupationOfTheFamily: string;
  otherPrimaryOccupationDetails: string;
  isFamilyInvolvedInWeavingOrHandloom: boolean;
  isFamilyCoveredUnderPOHI_LoomsScheme: boolean;
  fraClaimantStatus: string;
  fra_LandAmountInAcres: number | null;
  ownsHomesteadPattaLand: boolean;
  approximatePrivateLandHolding: string;
  isIrrigationFacilityAvailable: boolean;
  sourcesOfIrrigation: string;
  involvedInLivestockActivity: string;
}

export interface HouseholdFamilyMember {
  id?: number;
  uniqueId?: string | null;
  name: string;
  age: number | null;
  gender: string;
  educationalQualification: string;
  migratedInLast3Years: boolean;
  destinationState: string;
  sectorOfEngagementDuringMigration: string;
  periodOfMigration: string;
  monthlyRemittanceDuringMigration: number | null;
  interestInSkillDevelopment: boolean | null;
  relationshipWithHeadOfHousehold?: string | null;
  memberHasLabourCard?: boolean | null;
  memberCoveredUnderNSKY?: boolean | null;
  entryDate?: string | null;
}

// Initializer to create a blank form
export const createEmptyHousehold = (): Household => ({
  householdBasicProfile: {
    district: '', block: '', gramPanchayat: '', revenueVillage: '', hamlet: '',
    headOfTheHouseholdNameAsPerAadhar: '', headOfTheHouseholdGender: '', aadharNo: '', socialCategory: '',
    bankAccountNumber: '', bankName: '', ifscCodeOrBranch: '',
    womenMemberName: '', womenMemberAge: null, womenMemberMaritalStatus: '', womenMemberRelationshipWithHead: '',
    isWomenCoveredUnderSHG: false, isWomenCoveredUnderSubhadraYojana: false,
    totalFamilyMembers: null, hasRationCard: false, rationCardNumber: '', drinkingWaterSource: '',
    hasUjjwalaLPGConnection: false, hasLabourCard: false, isCoveredUnderNSKY: false, geoLocation: ''
  },
  householdEntitlement: {
    kishanSchemeCoverage: '', hasRuralHousingSchemeHouse: false, hasIndividualHouseholdLatrine: false,
    hasElectricityConnection: false, hasMGNREGSJobCard: false, fullJobCardNumber: '',
    hasJanDhanYojanaAccount: false, isCoveredUnderAyushmanBharat: false, isEnrolledUnderShramYogiMaandhan: false,
    isCoveredUnderPMJJBY: false, isCoveredUnderPMSBY: false, atalPension: false, oldagePension: false, widowPension: false, disabilityPension: false
  },
  householdMigrationStatus: {
    hasFamilyMemberMigratedLast3Years: false, takenAdvanceForMigrationFromMiddleman: false,
    minorChildrenAccompaniedMigration: false, womenMembersMigrated: false, familyContactMobileNo: '', respondentIdentity: ''
  },
  householdOccupationAndLand: {
    primaryOccupationOfTheFamily: '', otherPrimaryOccupationDetails: '', isFamilyInvolvedInWeavingOrHandloom: false,
    isFamilyCoveredUnderPOHI_LoomsScheme: false, fraClaimantStatus: '', fra_LandAmountInAcres: null,
    ownsHomesteadPattaLand: false, approximatePrivateLandHolding: '', isIrrigationFacilityAvailable: false,
    sourcesOfIrrigation: '', involvedInLivestockActivity: ''
  },
  householdFamilyMember: []
});
