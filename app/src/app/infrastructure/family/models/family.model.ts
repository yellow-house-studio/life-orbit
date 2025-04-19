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

export interface SafeFood {
  foodItem: string;
}

export interface FoodPreference {
  foodItem: string;
  status: FoodPreferenceStatus;
}

export enum AllergenSeverity {
  AvailableForOthers = 'AvailableForOthers',
  NotAllowed = 'NotAllowed'
}

export enum FoodPreferenceStatus {
  Include = 'Include',
  AvailableForOthers = 'AvailableForOthers',
  NotAllowed = 'NotAllowed'
} 