import { FamilyMember, AllergenSeverity, FoodPreferenceStatus } from './family.model';

describe('Family Models', () => {
  describe('FamilyMember', () => {
    it('should create a valid family member', () => {
      const member: FamilyMember = {
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
          { foodItem: 'rice', status: FoodPreferenceStatus.Include }
        ]
      };

      expect(member.id).toBe('123');
      expect(member.name).toBe('John Doe');
      expect(member.age).toBe(30);
      expect(member.allergies).toHaveLength(1);
      expect(member.safeFoods).toHaveLength(1);
      expect(member.foodPreferences).toHaveLength(1);
    });
  });

  describe('AllergenSeverity', () => {
    it('should have correct enum values', () => {
      expect(AllergenSeverity.AvailableForOthers).toBe('AvailableForOthers');
      expect(AllergenSeverity.NotAllowed).toBe('NotAllowed');
    });
  });

  describe('FoodPreferenceStatus', () => {
    it('should have correct enum values', () => {
      expect(FoodPreferenceStatus.Include).toBe('Include');
      expect(FoodPreferenceStatus.AvailableForOthers).toBe('AvailableForOthers');
      expect(FoodPreferenceStatus.NotAllowed).toBe('NotAllowed');
    });
  });
}); 