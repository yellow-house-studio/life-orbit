export interface FamilyMember {
  id: string;
  name: string;
  age: number;
  allergies: Allergy[];
  safeFoods: SafeFood[];
  foodPreferences: FoodPreference[];
}

export interface Allergy {
  allergen: string;
  severity: AllergenSeverity;
}

export enum AllergenSeverity {
  AvailableForOthers = 'Available for Others',
  NotAllowed = 'Not Allowed'
}

export interface SafeFood {
  foodItem: string;
}

export interface FoodPreference {
  preference: string;
  status: FoodPreferenceStatus;
}

export enum FoodPreferenceStatus {
  Include = 'Include',
  AvailableForOthers = 'Available for Others',
  NotAllowed = 'Not Allowed'
} 