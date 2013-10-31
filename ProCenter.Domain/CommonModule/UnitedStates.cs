#region Licence Header
// /*******************************************************************************
//  * Open Behavioral Health Information Technology Architecture (OBHITA.org)
//  * 
//  * Redistribution and use in source and binary forms, with or without
//  * modification, are permitted provided that the following conditions are met:
//  *     * Redistributions of source code must retain the above copyright
//  *       notice, this list of conditions and the following disclaimer.
//  *     * Redistributions in binary form must reproduce the above copyright
//  *       notice, this list of conditions and the following disclaimer in the
//  *       documentation and/or other materials provided with the distribution.
//  *     * Neither the name of the <organization> nor the
//  *       names of its contributors may be used to endorse or promote products
//  *       derived from this software without specific prior written permission.
//  * 
//  * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//  * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//  * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  * DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
//  * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//  * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//  * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//  * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//  ******************************************************************************/
#endregion
namespace ProCenter.Domain.CommonModule
{
    #region Using Statements

    using Lookups;

    #endregion

    [LookupRegistration(typeof(StateProvince))]
    public class UnitedStates : StateProvince
    {
        #region Static Fields

        private static readonly CodeSystem CodeSystem = CodeSystems.Obhita;

        /// <summary>
        ///     Alabama = 0.
        /// </summary>
        public static readonly UnitedStates Alabama = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "ALABAMA", codeSystem: CodeSystem, name: "ALABAMA" ),
                SortOrder = 0,
                Value = 0
            };

        /// <summary>
        ///     Alaska = 1.
        /// </summary>
        public static readonly UnitedStates Alaska = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "ALASKA", codeSystem: CodeSystem, name: "ALASKA" ),
                SortOrder = 1,
                Value = 1
            };

        /// <summary>
        ///     Arizona = 2.
        /// </summary>
        public static readonly UnitedStates Arizona = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "ARIZONA", codeSystem: CodeSystem, name: "ARIZONA" ),
                SortOrder = 2,
                Value = 2
            };

        /// <summary>
        ///     Arkansas = 3.
        /// </summary>
        public static readonly UnitedStates Arkansas = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "ARKANSAS", codeSystem: CodeSystem, name: "ARKANSAS" ),
                SortOrder = 3,
                Value = 3
            };

        /// <summary>
        ///     California = 4.
        /// </summary>
        public static readonly UnitedStates California = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "CALIFORNIA", codeSystem: CodeSystem, name: "CALIFORNIA" ),
                SortOrder = 4,
                Value = 4
            };

        /// <summary>
        ///     Colorado = 5.
        /// </summary>
        public static readonly UnitedStates Colorado = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "COLORADO", codeSystem: CodeSystem, name: "COLORADO" ),
                SortOrder = 5,
                Value = 5
            };

        /// <summary>
        ///     Connecticut = 6.
        /// </summary>
        public static readonly UnitedStates Connecticut = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "CONNECTICUT", codeSystem: CodeSystem, name: "CONNECTICUT" ),
                SortOrder = 6,
                Value = 6
            };

        /// <summary>
        ///     Delaware = 7.
        /// </summary>
        public static readonly UnitedStates Delaware = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "DELAWARE", codeSystem: CodeSystem, name: "DELAWARE" ),
                SortOrder = 7,
                Value = 7
            };

        /// <summary>
        ///     Florida = 8.
        /// </summary>
        public static readonly UnitedStates Florida = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "FLORIDA", codeSystem: CodeSystem, name: "FLORIDA" ),
                SortOrder = 8,
                Value = 8
            };

        /// <summary>
        ///     Georgia = 9.
        /// </summary>
        public static readonly UnitedStates Georgia = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "GEORGIA", codeSystem: CodeSystem, name: "GEORGIA" ),
                SortOrder = 9,
                Value = 9
            };

        /// <summary>
        ///     Hawaii = 10.
        /// </summary>
        public static readonly UnitedStates Hawaii = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "HAWAII", codeSystem: CodeSystem, name: "HAWAII" ),
                SortOrder = 10,
                Value = 10
            };

        /// <summary>
        ///     Idaho = 11.
        /// </summary>
        public static readonly UnitedStates Idaho = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "IDAHO", codeSystem: CodeSystem, name: "IDAHO" ),
                SortOrder = 11,
                Value = 11
            };

        /// <summary>
        ///     Illinois = 12.
        /// </summary>
        public static readonly UnitedStates Illinois = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "ILLINOIS", codeSystem: CodeSystem, name: "ILLINOIS" ),
                SortOrder = 12,
                Value = 12
            };

        /// <summary>
        ///     Indiana = 13.
        /// </summary>
        public static readonly UnitedStates Indiana = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "INDIANA", codeSystem: CodeSystem, name: "INDIANA" ),
                SortOrder = 13,
                Value = 13
            };

        /// <summary>
        ///     Iowa = 14.
        /// </summary>
        public static readonly UnitedStates Iowa = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "IOWA", codeSystem: CodeSystem, name: "IOWA" ),
                SortOrder = 14,
                Value = 14
            };

        /// <summary>
        ///     Kansas = 15.
        /// </summary>
        public static readonly UnitedStates Kansas = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "KANSAS", codeSystem: CodeSystem, name: "KANSAS" ),
                SortOrder = 15,
                Value = 15
            };

        /// <summary>
        ///     Kentucky = 16.
        /// </summary>
        public static readonly UnitedStates Kentucky = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "KENTUCKY", codeSystem: CodeSystem, name: "KENTUCKY" ),
                SortOrder = 16,
                Value = 16
            };

        /// <summary>
        ///     Louisiana = 17.
        /// </summary>
        public static readonly UnitedStates Louisiana = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "LOUISIANA", codeSystem: CodeSystem, name: "LOUISIANA" ),
                SortOrder = 17,
                Value = 17
            };

        /// <summary>
        ///     Maine = 18.
        /// </summary>
        public static readonly UnitedStates Maine = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "MAINE", codeSystem: CodeSystem, name: "MAINE" ),
                SortOrder = 18,
                Value = 18
            };

        /// <summary>
        ///     Maryland = 19.
        /// </summary>
        public static readonly UnitedStates Maryland = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "MARYLAND", codeSystem: CodeSystem, name: "MARYLAND" ),
                SortOrder = 19,
                Value = 19
            };

        /// <summary>
        ///     Massachusetts = 20.
        /// </summary>
        public static readonly UnitedStates Massachusetts = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "MASSACHUSETTS", codeSystem: CodeSystem, name: "MASSACHUSETTS" ),
                SortOrder = 20,
                Value = 20
            };

        /// <summary>
        ///     Michigan = 21.
        /// </summary>
        public static readonly UnitedStates Michigan = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "MICHIGAN", codeSystem: CodeSystem, name: "MICHIGAN" ),
                SortOrder = 21,
                Value = 21
            };

        /// <summary>
        ///     Minnesota = 22.
        /// </summary>
        public static readonly UnitedStates Minnesota = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "MINNESOTA", codeSystem: CodeSystem, name: "MINNESOTA" ),
                SortOrder = 22,
                Value = 22
            };

        /// <summary>
        ///     Mississippi = 23.
        /// </summary>
        public static readonly UnitedStates Mississippi = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "MISSISSIPPI", codeSystem: CodeSystem, name: "MISSISSIPPI" ),
                SortOrder = 23,
                Value = 23
            };

        /// <summary>
        ///     Missouri = 24.
        /// </summary>
        public static readonly UnitedStates Missouri = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "MISSOURI", codeSystem: CodeSystem, name: "MISSOURI" ),
                SortOrder = 24,
                Value = 24
            };

        /// <summary>
        ///     Montana = 25.
        /// </summary>
        public static readonly UnitedStates Montana = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "MONTANA", codeSystem: CodeSystem, name: "MONTANA" ),
                SortOrder = 25,
                Value = 25
            };

        /// <summary>
        ///     Nebraska = 26.
        /// </summary>
        public static readonly UnitedStates Nebraska = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "NEBRASKA", codeSystem: CodeSystem, name: "NEBRASKA" ),
                SortOrder = 26,
                Value = 26
            };

        /// <summary>
        ///     Nevada = 27.
        /// </summary>
        public static readonly UnitedStates Nevada = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "NEVADA", codeSystem: CodeSystem, name: "NEVADA" ),
                SortOrder = 27,
                Value = 27
            };

        /// <summary>
        ///     NewHampshire = 28.
        /// </summary>
        public static readonly UnitedStates NewHampshire = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "NEW HAMPSHIRE", codeSystem: CodeSystem, name: "NEW HAMPSHIRE" ),
                SortOrder = 28,
                Value = 28
            };

        /// <summary>
        ///     NewJersey = 29.
        /// </summary>
        public static readonly UnitedStates NewJersey = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "NEW JERSEY", codeSystem: CodeSystem, name: "NEW JERSEY" ),
                SortOrder = 29,
                Value = 29
            };

        /// <summary>
        ///     NewMexico = 30.
        /// </summary>
        public static readonly UnitedStates NewMexico = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "NEW MEXICO", codeSystem: CodeSystem, name: "NEW MEXICO" ),
                SortOrder = 30,
                Value = 30
            };

        /// <summary>
        ///     NewYork = 31.
        /// </summary>
        public static readonly UnitedStates NewYork = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "NEW YORK", codeSystem: CodeSystem, name: "NEW YORK" ),
                SortOrder = 31,
                Value = 31
            };

        /// <summary>
        ///     NorthCarolina = 32.
        /// </summary>
        public static readonly UnitedStates NorthCarolina = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "NORTH CAROLINA", codeSystem: CodeSystem, name: "NORTH CAROLINA" ),
                SortOrder = 32,
                Value = 32
            };

        /// <summary>
        ///     NorthDakota = 33.
        /// </summary>
        public static readonly UnitedStates NorthDakota = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "NORTH DAKOTA", codeSystem: CodeSystem, name: "NORTH DAKOTA" ),
                SortOrder = 33,
                Value = 33
            };

        /// <summary>
        ///     Ohio = 34.
        /// </summary>
        public static readonly UnitedStates Ohio = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "OHIO", codeSystem: CodeSystem, name: "OHIO" ),
                SortOrder = 34,
                Value = 34
            };

        /// <summary>
        ///     Oklahoma = 35.
        /// </summary>
        public static readonly UnitedStates Oklahoma = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "OKLAHOMA", codeSystem: CodeSystem, name: "OKLAHOMA" ),
                SortOrder = 35,
                Value = 35
            };

        /// <summary>
        ///     Oregon = 36.
        /// </summary>
        public static readonly UnitedStates Oregon = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "OREGON", codeSystem: CodeSystem, name: "OREGON" ),
                SortOrder = 36,
                Value = 36
            };

        /// <summary>
        ///     Pennsylvania = 37.
        /// </summary>
        public static readonly UnitedStates Pennsylvania = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "PENNSYLVANIA", codeSystem: CodeSystem, name: "PENNSYLVANIA" ),
                SortOrder = 37,
                Value = 37
            };

        /// <summary>
        ///     RhodeIsland = 38.
        /// </summary>
        public static readonly UnitedStates RhodeIsland = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "RHODE ISLAND", codeSystem: CodeSystem, name: "RHODE ISLAND" ),
                SortOrder = 38,
                Value = 38
            };

        /// <summary>
        ///     SouthCarolina = 39.
        /// </summary>
        public static readonly UnitedStates SouthCarolina = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "SOUTH CAROLINA", codeSystem: CodeSystem, name: "SOUTH CAROLINA" ),
                SortOrder = 39,
                Value = 39
            };

        /// <summary>
        ///     SouthDakota = 40.
        /// </summary>
        public static readonly UnitedStates SouthDakota = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "SOUTH DAKOTA", codeSystem: CodeSystem, name: "SOUTH DAKOTA" ),
                SortOrder = 40,
                Value = 40
            };

        /// <summary>
        ///     Tennessee = 41.
        /// </summary>
        public static readonly UnitedStates Tennessee = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "TENNESSEE", codeSystem: CodeSystem, name: "TENNESSEE" ),
                SortOrder = 41,
                Value = 41
            };

        /// <summary>
        ///     Texas = 42.
        /// </summary>
        public static readonly UnitedStates Texas = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "TEXAS", codeSystem: CodeSystem, name: "TEXAS" ),
                SortOrder = 42,
                Value = 42
            };

        /// <summary>
        ///     Utah = 43.
        /// </summary>
        public static readonly UnitedStates Utah = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "UTAH", codeSystem: CodeSystem, name: "UTAH" ),
                SortOrder = 43,
                Value = 43
            };

        /// <summary>
        ///     Vermont = 44.
        /// </summary>
        public static readonly UnitedStates Vermont = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "VERMONT", codeSystem: CodeSystem, name: "VERMONT" ),
                SortOrder = 44,
                Value = 44
            };

        /// <summary>
        ///     Virginia = 45.
        /// </summary>
        public static readonly UnitedStates Virginia = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "VIRGINIA", codeSystem: CodeSystem, name: "VIRGINIA" ),
                SortOrder = 45,
                Value = 45
            };

        /// <summary>
        ///     Washington = 46.
        /// </summary>
        public static readonly UnitedStates Washington = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "WASHINGTON", codeSystem: CodeSystem, name: "WASHINGTON" ),
                SortOrder = 46,
                Value = 46
            };

        /// <summary>
        ///     WestVirginia = 47.
        /// </summary>
        public static readonly UnitedStates WestVirginia = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "WEST VIRGINIA", codeSystem: CodeSystem, name: "WEST VIRGINIA" ),
                SortOrder = 47,
                Value = 47
            };

        /// <summary>
        ///     Wisconsin = 48.
        /// </summary>
        public static readonly UnitedStates Wisconsin = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "WISCONSIN", codeSystem: CodeSystem, name: "WISCONSIN" ),
                SortOrder = 48,
                Value = 48
            };

        /// <summary>
        ///     Wyoming = 49.
        /// </summary>
        public static readonly UnitedStates Wyoming = new UnitedStates
            {
                CodedConcept = new CodedConcept ( code: "WYOMING", codeSystem: CodeSystem, name: "WYOMING" ),
                SortOrder = 49,
                Value = 49
            };

        #endregion
    }
}