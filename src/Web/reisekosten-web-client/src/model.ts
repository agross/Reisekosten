export type Reise = {
  anfang: Date;
  grund: string;
  zielort: string;
  pauschale: Number;
};

export type Bericht = {
  reisen: Reise[];
  summe: Number;
};
