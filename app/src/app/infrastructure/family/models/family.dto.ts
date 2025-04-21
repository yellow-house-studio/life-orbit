export interface FamilyMemberResponse {
  id: string;
  name: string;
  age: number;
  allergies: AllergyResponse[];
  safeFoods: SafeFoodResponse[];
  foodPreferences: FoodPreferenceResponse[];
}

export interface CreateFamilyMemberRequest {
  name: string;
  age: number;
}

export interface CreateCompleteFamilyMemberRequest {
  name: string;
  age: number;
  allergies: AllergyRequest[];
  safeFoods: SafeFoodRequest[];
  foodPreferences: FoodPreferenceRequest[];
}

export interface AllergyResponse {
  allergen: string;
  severity: 'AvailableForOthers' | 'NotAllowed';
}

export interface AllergyRequest {
  allergen: string;
  severity: 'AvailableForOthers' | 'NotAllowed';
}

export interface SafeFoodResponse {
  foodItem: string;
}

export interface SafeFoodRequest {
  foodItem: string;
}

export interface FoodPreferenceResponse {
  foodItem: string;
  status: 'Include' | 'AvailableForOthers' | 'NotAllowed';
}

export interface FoodPreferenceRequest {
  foodItem: string;
  status: 'Include' | 'AvailableForOthers' | 'NotAllowed';
} 