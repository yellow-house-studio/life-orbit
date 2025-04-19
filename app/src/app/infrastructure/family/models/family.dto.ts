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

export interface AllergyResponse {
  allergen: string;
  severity: 'AvailableForOthers' | 'NotAllowed';
}

export interface SafeFoodResponse {
  foodItem: string;
}

export interface FoodPreferenceResponse {
  foodItem: string;
  status: 'Include' | 'AvailableForOthers' | 'NotAllowed';
} 