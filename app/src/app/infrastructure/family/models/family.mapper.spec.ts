import { 
  mapFamilyMemberResponseToModel, 
  mapAllergyResponseToModel,
  mapSafeFoodResponseToModel,
  mapFoodPreferenceResponseToModel,
  createFamilyMemeberRequest,
  mapAllergyToAllergyRequest,
  mapSafeFoodToSafeFoodRequest,
  mapFoodPreferenceToFoodPreferenceRequest
} from './family.mapper';
import { AllergenSeverity, FoodPreferenceStatus } from './family.model';
import { FamilyMemberResponse } from './family.dto';

describe('Family Mappers', () => {
  describe('mapAllergyResponseToModel', () => {
    it('should map allergy response to model', () => {
      const response = {
        allergen: 'peanuts',
        severity: 'NotAllowed' as const
      };

      const result = mapAllergyResponseToModel(response);

      expect(result).toEqual({
        allergen: 'peanuts',
        severity: AllergenSeverity.NotAllowed
      });
    });
  });

  describe('mapSafeFoodResponseToModel', () => {
    it('should map safe food response to model', () => {
      const response = {
        foodItem: 'pasta'
      };

      const result = mapSafeFoodResponseToModel(response);

      expect(result).toEqual({
        foodItem: 'pasta'
      });
    });
  });

  describe('mapFoodPreferenceResponseToModel', () => {
    it('should map food preference response to model', () => {
      const response = {
        foodItem: 'rice',
        status: 'Include' as const
      };

      const result = mapFoodPreferenceResponseToModel(response);

      expect(result).toEqual({
        preference: 'rice',
        status: FoodPreferenceStatus.Include
      });
    });
  });

  describe('mapFamilyMemberResponseToModel', () => {
    it('should map family member response to model', () => {
      const response: FamilyMemberResponse = {
        id: '123',
        name: 'John Doe',
        age: 30,
        allergies: [
          { allergen: 'peanuts', severity: 'NotAllowed' }
        ],
        safeFoods: [
          { foodItem: 'pasta' }
        ],
        foodPreferences: [
          { foodItem: 'rice', status: 'Include' }
        ]
      };

      const result = mapFamilyMemberResponseToModel(response);

      expect(result).toEqual({
        id: '123',
        name: 'John Doe',
        age: 30,
        allergies: [
          { allergen: 'peanuts', severity: AllergenSeverity.NotAllowed }
        ],
        safeFoods: [
          { foodItem: 'pasta' }
        ],
        foodPreferences: [
          { preference: 'rice', status: FoodPreferenceStatus.Include }
        ]
      });
    });

    it('should map empty arrays correctly', () => {
      const response: FamilyMemberResponse = {
        id: '123',
        name: 'John Doe',
        age: 30,
        allergies: [],
        safeFoods: [],
        foodPreferences: []
      };

      const result = mapFamilyMemberResponseToModel(response);

      expect(result.allergies).toEqual([]);
      expect(result.safeFoods).toEqual([]);
      expect(result.foodPreferences).toEqual([]);
    });
  });

  describe('mapAllergyToAllergyRequest', () => {
    it('should map allergy model to request with AvailableForOthers severity', () => {
      const model = {
        allergen: 'peanuts',
        severity: AllergenSeverity.AvailableForOthers
      };

      const result = mapAllergyToAllergyRequest(model);

      expect(result).toEqual({
        allergen: 'peanuts',
        severity: 'AvailableForOthers'
      });
    });

    it('should map allergy model to request with NotAllowed severity', () => {
      const model = {
        allergen: 'peanuts',
        severity: AllergenSeverity.NotAllowed
      };

      const result = mapAllergyToAllergyRequest(model);

      expect(result).toEqual({
        allergen: 'peanuts',
        severity: 'NotAllowed'
      });
    });
  });

  describe('mapSafeFoodToSafeFoodRequest', () => {
    it('should map safe food model to request', () => {
      const model = {
        foodItem: 'pasta'
      };

      const result = mapSafeFoodToSafeFoodRequest(model);

      expect(result).toEqual({
        foodItem: 'pasta'
      });
    });
  });

  describe('mapFoodPreferenceToFoodPreferenceRequest', () => {
    it('should map food preference model to request with Include status', () => {
      const model = {
        preference: 'rice',
        status: FoodPreferenceStatus.Include
      };

      const result = mapFoodPreferenceToFoodPreferenceRequest(model);

      expect(result).toEqual({
        foodItem: 'rice',
        status: 'Include'
      });
    });

    it('should map food preference model to request with AvailableForOthers status', () => {
      const model = {
        preference: 'rice',
        status: FoodPreferenceStatus.AvailableForOthers
      };

      const result = mapFoodPreferenceToFoodPreferenceRequest(model);

      expect(result).toEqual({
        foodItem: 'rice',
        status: 'AvailableForOthers'
      });
    });

    it('should map food preference model to request with NotAllowed status', () => {
      const model = {
        preference: 'rice',
        status: FoodPreferenceStatus.NotAllowed
      };

      const result = mapFoodPreferenceToFoodPreferenceRequest(model);

      expect(result).toEqual({
        foodItem: 'rice',
        status: 'NotAllowed'
      });
    });
  });

  describe('createFamilyMemeberRequest', () => {
    it('should create a complete family member request', () => {
      const name = 'John Doe';
      const age = 30;
      const allergies = [
        { allergen: 'peanuts', severity: AllergenSeverity.NotAllowed }
      ];
      const safeFoods = [
        { foodItem: 'pasta' }
      ];
      const foodPreferences = [
        { preference: 'rice', status: FoodPreferenceStatus.Include }
      ];

      const result = createFamilyMemeberRequest(name, age, allergies, safeFoods, foodPreferences);

      expect(result).toEqual({
        name: 'John Doe',
        age: 30,
        allergies: [
          { allergen: 'peanuts', severity: 'NotAllowed' }
        ],
        safeFoods: [
          { foodItem: 'pasta' }
        ],
        foodPreferences: [
          { foodItem: 'rice', status: 'Include' }
        ]
      });
    });

    it('should create a request with empty arrays', () => {
      const result = createFamilyMemeberRequest('John Doe', 30, [], [], []);

      expect(result).toEqual({
        name: 'John Doe',
        age: 30,
        allergies: [],
        safeFoods: [],
        foodPreferences: []
      });
    });
  });
}); 